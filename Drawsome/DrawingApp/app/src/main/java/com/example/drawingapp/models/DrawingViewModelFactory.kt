package com.example.drawingapp.models

import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import com.example.drawingapp.data.ImageRepository

class DrawingViewModelFactory(
    private val repository: ImageRepository,
    private val screenWidth: Int,
) : ViewModelProvider.Factory {

    override fun <T : ViewModel> create(modelClass: Class<T>): T {
        if (modelClass.isAssignableFrom(DrawingViewModel::class.java)) {
            @Suppress("UNCHECKED_CAST")
            return DrawingViewModel(repository, screenWidth) as T
        }
        throw IllegalArgumentException("Unknown ViewModel class")
    }
}