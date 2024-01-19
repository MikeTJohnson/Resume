package com.example.drawingapp.models

import android.graphics.Bitmap
import android.graphics.Canvas
import android.graphics.Color
import android.graphics.Paint
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.example.drawingapp.data.Drawing
import com.example.drawingapp.data.ImageData
import com.example.drawingapp.data.ImageRepository
import java.util.UUID

enum class StrokeStyle {
    CIRCLE, LINE, SQUARE, MARBLE
}

class DrawingViewModel(private val repository: ImageRepository, private val screenWidth: Int ) : ViewModel() {
    private val _bitmap = MutableLiveData(Bitmap.createBitmap(screenWidth, screenWidth, Bitmap.Config.ARGB_8888))
    val bitmap = _bitmap as LiveData<Bitmap>

    //keep track of bitmap filename for saving. The default is an random UUID initially
    private val _bitmapFileName = MutableLiveData(UUID.randomUUID().toString() + ".png")
    val bitmapFileName = _bitmapFileName as LiveData<String>

    private val _imageData = MutableLiveData<ImageData?>()
    val imageData = _imageData as LiveData<ImageData?>

    val allImages: LiveData<List<ImageData>> = repository.allImageData

    //data from gravity sensor when active
    private val _gravityData = MutableLiveData<FloatArray>()
    val gravityData = _gravityData as LiveData<FloatArray>
    fun updateGravityData(data: FloatArray){
        _gravityData.value = data
    }

    private val _importImageData = MutableLiveData<List<Drawing>>()
    val importImageData = _importImageData as LiveData<List<Drawing>>

//    private val _importImages = MutableLiveData<Array<Bitmap>>()
//    val importImages = _importImages as LiveData<Array<Bitmap>>
    fun updateImportImageData(data: List<Drawing>) {
        _importImageData.value = data
    }



    private val _penColor = MutableLiveData(Paint().apply{
        color = Color.BLACK
        style = Paint.Style.STROKE
        strokeWidth = 5f})
    val penColor = _penColor as LiveData<Paint>
    private val _currentStrokeStyle = MutableLiveData(StrokeStyle.LINE)
    val currentStrokeStyle = _currentStrokeStyle as LiveData<StrokeStyle>


    //delete image from the database
    suspend fun deleteImage(data : ImageData) {
        repository.deleteImage(data)
    }

    //save new image in database
    suspend fun saveNewImageData(bitmap: Bitmap, fileName: String){
        val insertedId = repository.saveNewImage(bitmap, fileName)
        _imageData.value = repository.getImageById(insertedId)
    }

    //update image in database
    suspend fun updateDatabaseImageData(imageData: ImageData, currentBitmap: Bitmap){
        repository.updateImage(imageData, currentBitmap)
    }

    //update filename for saving, call this when selecting an image or creating a new one
    fun updateBitmapFileName(fileName: String){
        _bitmapFileName.value = fileName
    }

    fun updateImageData(imageData: ImageData){
        _imageData.value = imageData
    }

    //set bitmap background to white, then load bitmap when selecting old drawing
    fun clearBitmap(newBitmap: Bitmap){
        val currentBitmap = bitmap.value
        currentBitmap?.apply {
            val canvas = Canvas(this)
            canvas.drawColor(Color.WHITE)
            canvas.drawBitmap(newBitmap, 0f, 0f, null)
            _bitmap.value = this
        }
        resetDrawingTools()
    }

    fun updateBitmap(newBitmap: Bitmap) {
        val currentBitmap = bitmap.value
        currentBitmap?.apply {
            // Copy the contents of the new bitmap to the current bitmap
            val canvas = Canvas(this)
            canvas.drawBitmap(newBitmap, 0f, 0f, null)
            _bitmap.value = this
        }
    }

    fun updateStrokeWidth(size: Float){
        val currentPen = penColor.value
        currentPen?.strokeWidth = size
        _penColor.value = currentPen!!
    }

    fun updatePenColor(color : Int){
        val currentPen = penColor.value
        currentPen?.color = color
        _penColor.value = currentPen!!
    }

    fun updateStrokeStyle(style : StrokeStyle){
        _currentStrokeStyle.value = style
    }

    //call this when creating a new image
    fun createNewDrawing(){
        val newBitmap = Bitmap.createBitmap(screenWidth, screenWidth, Bitmap.Config.ARGB_8888)
        clearBitmap(newBitmap)
        _bitmapFileName.value = UUID.randomUUID().toString() + ".png"
        _imageData.value = null
        resetDrawingTools()
    }

    fun importNewDrawing(importBitmap : Bitmap){
        // Android was creating a hardware bitmap, which is immutable
        // Needed to convert to a software bitmap which is mutable
        val softwareBitmap = importBitmap.copy(Bitmap.Config.ARGB_8888, true)
        clearBitmap(softwareBitmap)
        _bitmapFileName.value = UUID.randomUUID().toString() + ".png"
        _imageData.value = null
        resetDrawingTools()
    }

    //reset drawing tools to default when loading image or creating new image
    private fun resetDrawingTools(){
        _currentStrokeStyle.value = StrokeStyle.LINE
        _penColor.value = Paint().apply{
            color = Color.BLACK
            style = Paint.Style.STROKE
            strokeWidth = 5f}
    }
}
