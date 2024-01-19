package com.example.drawingapp.models

import android.content.Context
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import com.example.drawingapp.data.Drawing
import com.example.drawingapp.data.SharingRepository
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.MutableStateFlow

class SharingViewModelFactory(
    private val repository: SharingRepository
) : ViewModelProvider.Factory {

    override fun <T : ViewModel> create(modelClass: Class<T>): T {
        if (modelClass.isAssignableFrom(SharingViewModel::class.java)) {
            @Suppress("UNCHECKED_CAST")
            return SharingViewModel(repository) as T
        }
        throw IllegalArgumentException("Unknown ViewModel class")
    }
}

class SharingViewModel(private val repository: SharingRepository) : ViewModel() {
    private val _importImageData = MutableLiveData<List<Drawing>>()
    val importImageData = _importImageData as LiveData<List<Drawing>>

    private val _toastMessage = MutableStateFlow("")
    val toastMessage: Flow<String> = _toastMessage

    private val _loggedIn = MutableLiveData(false)
    val loggedIn = _loggedIn as LiveData<Boolean>

    private var _serverRunning = MutableLiveData(false)
    val serverRunning = _serverRunning as LiveData<Boolean>

    fun updateLoggedIn(loggedIn: Boolean) {
        _loggedIn.value = loggedIn
    }

    fun setToastMessage(toast: String) {
        _toastMessage.value = toast
    }

    fun updateImportImageData(data: List<Drawing>) {
        _importImageData.value = data
    }
    suspend fun uploadToServer(file : String) {
        repository.uploadToServer(file, this)
    }
    suspend fun requestDrawingList(context: Context) {
        repository.requestDrawingList(context, this)
    }
    suspend fun deleteDrawing(context: Context, drawing: Drawing) {
        repository.deleteDrawing(drawing, context, this)
    }
    fun setServerRunning(isRunning: Boolean) {
        _serverRunning.postValue(isRunning)
    }
    fun updateServerStatus(context: Context) {
        repository.checkServerRunning(context, this)
    }

}
