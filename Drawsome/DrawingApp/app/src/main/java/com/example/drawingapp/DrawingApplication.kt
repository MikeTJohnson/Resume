package com.example.drawingapp

import android.app.Application
import com.example.drawingapp.data.ImageDatabase
import com.example.drawingapp.data.ImageRepository
import com.example.drawingapp.data.SharingRepository
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.SupervisorJob

class DrawingApplication: Application() {
    private val scope = CoroutineScope(SupervisorJob())
    private val db by lazy { ImageDatabase.getDatabase(applicationContext)}
    val imageRepo by lazy { ImageRepository(scope, db.imageDao()) }
    val sharingRepo by lazy { SharingRepository() }
}