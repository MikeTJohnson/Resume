package com.example.plugins

import com.example.plugins.Drawing.file_path
import com.example.plugins.Drawing.user_name
import com.example.plugins.Drawing.user_uid
import com.google.firebase.auth.FirebaseAuth
import com.google.firebase.auth.FirebaseAuthException
import io.ktor.http.*
import io.ktor.http.auth.*
import io.ktor.http.content.*
import io.ktor.resources.*
import io.ktor.server.application.*
import io.ktor.server.auth.*
import io.ktor.server.http.content.*
import io.ktor.server.request.*
import io.ktor.server.resources.*
import io.ktor.server.resources.Resources
import io.ktor.server.resources.get
import io.ktor.server.resources.post
import io.ktor.server.response.*
import io.ktor.server.routing.*
import kotlinx.serialization.Serializable
import org.jetbrains.exposed.sql.*
import org.jetbrains.exposed.sql.SqlExpressionBuilder.eq
import org.jetbrains.exposed.sql.transactions.experimental.newSuspendedTransaction
import java.io.File

fun Application.configureRouting() {
    install(Resources)
    routing {
        // GET http://localhost:8080/drawings HTTP/1.1
        get<DrawingsResource> {
            val authHeader: HttpAuthHeader? = call.request.parseAuthorizationHeader()
            if (authHeader == null){
                call.respondText(
                    "Unauthorized Request",
                    status=HttpStatusCode.Unauthorized
                )
                return@get
            }
            val idToken:String = authHeader.toString()
            try {
                val decodedToken = FirebaseAuth.getInstance().verifyIdToken(idToken)
                call.respond(
                    newSuspendedTransaction {
                        Drawing
                            .selectAll()
                            .orderBy(Drawing.timestamp)
                            .map {
                                DrawingData(
                                    it[file_path],
                                    it[user_name],
                                    it[user_uid] == decodedToken.uid,
                                )
                            }
                    }
                )
            } catch (error: FirebaseAuthException){
                call.respondText(
                    text = "Unauthorized Request",
                    status= HttpStatusCode.Unauthorized
                )
            }
        }

        // GET http://localhost:8080/drawing/{fileName} HTTP/1.1
        get<DrawingResource> {
            val authHeader: HttpAuthHeader? = call.request.parseAuthorizationHeader()
            if (authHeader == null){
                call.respondText(
                    "Unauthorized Request",
                    status=HttpStatusCode.Unauthorized
                )
            }
            val idToken:String = authHeader.toString()
            try {
                FirebaseAuth.getInstance().verifyIdToken(idToken)
                val file = File("src/main/resources/public/${it.fileName}")
                if (file.exists()) {
                    call.respondFile(file)
                } else {
                    call.respondText(
                        text = "File not found",
                        status= HttpStatusCode.NotFound
                    )
                }
            }catch (error: FirebaseAuthException){
                call.respondText(
                    text = "Unauthorized Request",
                    status= HttpStatusCode.Unauthorized
                )
            }
        }

        /*
            * This code is based on the following:
            * https://ktor.io/docs/requests.html#form_data
        */
        // POST http://localhost:8080/upload HTTP/1.1
        post<UploadResource> {
            val authHeader: HttpAuthHeader? = call.request.parseAuthorizationHeader()
            if (authHeader == null){
                call.respondText(
                    "Unauthorized Request",
                    status=HttpStatusCode.Unauthorized
                )
            }
            val idToken:String = authHeader.toString()
            var fileName = ""
            try {
                val decodedToken = FirebaseAuth.getInstance().verifyIdToken(idToken)
                val multipartData = call.receiveMultipart()
                multipartData.forEachPart { part ->
                    when (part) {
                        is PartData.FileItem -> {
                            fileName = part.originalFileName as String
                            // println(System.getProperty("user.dir") + ": Printing PWD")
                            val file = File("src/main/resources/public/$fileName")
                            if (!file.exists()) {
                                val fileBytes = part.streamProvider().readBytes()
                                file.writeBytes(fileBytes)
                                newSuspendedTransaction {
                                    Drawing.insert {
                                        it[file_path] = fileName
                                        it[timestamp] = System.currentTimeMillis()
                                        it[user_uid] = decodedToken.uid
                                        it[user_name] = decodedToken.name
                                    }
                                }
                            }
                        }
                        else -> {}
                    }
                    part.dispose()
                }
                call.respondText("Image uploaded to 'public/$fileName'")
            } catch (error: FirebaseAuthException){
                call.respondText(
                    text = "Unauthorized Request",
                    status= HttpStatusCode.Unauthorized
                )
            }
        }

    // DELETE http://localhost:8080/delete/{fileName} HTTP/1.1
        delete<DeleteFileResource> {
            val authHeader: HttpAuthHeader? = call.request.parseAuthorizationHeader()
            if (authHeader == null) {
                call.respondText(
                    "Unauthorized Request",
                    status = HttpStatusCode.Unauthorized
                )
            } else {
                val idToken: String = authHeader.toString()
                val fileName = it.fileName

                try {
                    val decodedToken = FirebaseAuth.getInstance().verifyIdToken(idToken)

                    newSuspendedTransaction {
                        val matchingDrawing = Drawing.select {
                            user_uid eq decodedToken.uid
                            file_path eq fileName
                        }.singleOrNull()

                        if (matchingDrawing != null) {
                            Drawing.deleteWhere {
                                user_uid eq decodedToken.uid
                                file_path eq fileName
                            }
                            File("src/main/resources/public/$fileName").delete()
                            call.respondText("$fileName deleted")
                        } else {
                            call.respondText(
                                "Drawing not found or you don't have ownership",
                                status = HttpStatusCode.NotFound
                            )
                        }
                    }
                } catch (error: FirebaseAuthException) {
                    call.respondText(
                        text = "Unauthorized Request",
                        status = HttpStatusCode.Unauthorized
                    )
                }
            }
        }
        get<Test> {
            call.respondText(
                text = "Server is Running!"
            )
        }

        staticResources("/", "public", index = "index.html")
    }
}

@Serializable data class DrawingData(val filePath: String, val userName: String?, val canDelete: Boolean) // allow userName to be null

@Resource("/delete/{fileName}")
class DeleteFileResource(val fileName: String)
@Resource("/upload")
class UploadResource()
@Resource("/drawings")
class DrawingsResource()
@Resource("/drawing/{fileName}")
class DrawingResource(val fileName: String)
@Resource("/test")
class Test()