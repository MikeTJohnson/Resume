package com.example.drawingapp.data

import android.content.Context
import android.util.Log
import com.android.volley.NetworkResponse
import com.android.volley.Request
import com.android.volley.Response
import com.android.volley.toolbox.HttpHeaderParser
import com.android.volley.toolbox.JsonArrayRequest
import com.android.volley.toolbox.StringRequest
import com.android.volley.toolbox.Volley
import com.example.drawingapp.models.SharingViewModel
import com.example.drawingapp.utils.IdToken
import com.google.gson.Gson
import io.ktor.client.HttpClient
import io.ktor.client.engine.cio.CIO
import io.ktor.client.request.forms.formData
import io.ktor.client.request.forms.submitFormWithBinaryData
import io.ktor.client.request.headers
import io.ktor.client.statement.HttpResponse
import io.ktor.http.Headers
import io.ktor.http.HttpHeaders
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import java.io.File

class SharingRepository {
    /// https://ktor.io/docs/requests.html#form_data
    suspend fun uploadToServer(file : String, viewModel: SharingViewModel) {
        var responseMessage = ""
        withContext(Dispatchers.IO) {
            val filename = file.split("/").last()
            val token = IdToken.getIdToken() ?: return@withContext
            val client = HttpClient(CIO)
            val response: HttpResponse = client.submitFormWithBinaryData(
                url = "http://10.0.2.2:8080/upload",
                formData = formData {
                    append("image", File(file).readBytes(), Headers.build {
                        append(HttpHeaders.ContentDisposition, "filename=$filename")
                    })
                }) {
                headers {
                    append(HttpHeaders.Authorization, token)
                }
            }
            when (response.status.value) {
                200 -> {
                    Log.i("upload", "Successfully uploaded $filename")
                    responseMessage = "Successfully uploaded image!"
                }
                400 -> {
                    Log.i("upload", "Bad request to upload $filename")
                    responseMessage = "Bad request to upload image!"
                }
                401 -> {
                    Log.i("upload", "Unauthorized to upload $filename")
                    responseMessage = "Unauthorized to upload image!"
                }
                500 -> {
                    Log.i("upload", "Server error on upload of $filename")
                    responseMessage = "Server error on upload of image!"
                }
                else -> {
                    Log.i("upload", "Failed to upload $filename")
                    responseMessage = "Failed to upload image!"
                }
            }
            Log.d("upload", "end of uploadToServer")
        }
        withContext(Dispatchers.Main) {
            viewModel.setToastMessage(responseMessage)
        }
    }
    suspend fun requestDrawingList(context: Context, viewModel: SharingViewModel){
        var responseMessage = ""
        withContext(Dispatchers.IO) {
            val queue = Volley.newRequestQueue(context)
            val url = "http://10.0.2.2:8080/drawings"
            val token = IdToken.getIdToken() ?: return@withContext
            val request: JsonArrayRequest = object : JsonArrayRequest(
                Method.GET,
                url, null, Response.Listener { response ->
                    val gson = Gson() // Convert JSON here rather than the viewModel
                    val drawings = gson.fromJson(response.toString(), Array<Drawing>::class.java)
                    Log.i("response", "response %s".format(response.toString()))
                    viewModel.updateImportImageData(drawings.toList())
                },
                Response.ErrorListener { error ->
                    Log.i("error", error.toString())
                    responseMessage = when(error.networkResponse.statusCode){
                        400 -> "Bad request"
                        401 -> "Unauthorized"
                        500 -> "Server error"
                        else -> "Unknown error"
                    }
                }) {
                override fun getHeaders(): Map<String, String> {
                    val params = HashMap<String, String>()
                    params.putAll(super.getHeaders())
                    params["Authorization"] = token
                    return params
                }
            }
            withContext(Dispatchers.Main) {
                viewModel.setToastMessage(responseMessage)
            }
            queue.add(request)
        }
    }
    suspend fun deleteDrawing(drawingData: Drawing, context: Context, viewModel: SharingViewModel) {
        var responseMessage = "Successfully deleted image!"
        withContext(Dispatchers.IO) {
            val queue = Volley.newRequestQueue(context)
            val url = "http://10.0.2.2:8080/delete/${drawingData.filePath}"
            val token = IdToken.getIdToken() ?: return@withContext
            val request: StringRequest = object : StringRequest(
                Method.DELETE,
                url,
                Response.Listener { response ->
                    Log.i("deleteDrawing response", response.toString())
                },
                Response.ErrorListener { error ->
                    // TODO: Do something with the error like  handle status code specific errors
                    Log.i("deleteDrawing error", error.toString())
                    responseMessage = when(error.networkResponse.statusCode){
                        400 -> "Bad request"
                        401 -> "Unauthorized"
                        500 -> "Server error"
                        else -> "Unknown error"
                    }
                }
            ) {

                override fun getHeaders(): Map<String, String> {
                    val params = HashMap<String, String>()
                    params.putAll(super.getHeaders())
                    params["Authorization"] = token
                    return params
                }

                // Implement parseNetworkResponse to avoid Volley parsing errors
                override fun parseNetworkResponse(response: NetworkResponse): Response<String> {
                    return Response.success("", HttpHeaderParser.parseCacheHeaders(response))
                }
            }
            withContext(Dispatchers.Main) {
                viewModel.setToastMessage(responseMessage)
            }
            queue.add(request)
        }
    }
    fun checkServerRunning(context: Context, viewModel: SharingViewModel) {
        val queue = Volley.newRequestQueue(context)
        val url = "http://10.0.2.2:8080/test"

        val request = StringRequest(
            Request.Method.GET,
            url,
            { response ->
                Log.i("isServerRunning response", response.toString())
                viewModel.setServerRunning(true)
            },
            { error ->
                Log.i("isServerRunning error", error.toString())
                viewModel.setServerRunning(false)
            }
        )
        queue.add(request)
    }

}
