package com.example.drawingapp.utils

import com.google.firebase.auth.FirebaseAuth
import com.google.firebase.auth.GetTokenResult
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.tasks.await
import kotlinx.coroutines.withContext

class IdToken {
    companion object{
        suspend fun getIdToken(): String? {
            return withContext(Dispatchers.IO) {
                val user = FirebaseAuth.getInstance().currentUser
                if (user != null) {
                    try {
                        val tokenResult: GetTokenResult = user.getIdToken(false).await()
                        return@withContext tokenResult.token
                    } catch (e: Exception) {
                        e.printStackTrace()
                        return@withContext null
                    }
                }
                return@withContext null
            }
        }
    }
}