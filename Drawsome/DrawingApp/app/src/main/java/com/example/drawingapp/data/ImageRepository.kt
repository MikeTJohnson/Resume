package com.example.drawingapp.data

import android.graphics.Bitmap
import android.media.ThumbnailUtils
import androidx.lifecycle.asLiveData
import kotlinx.coroutines.CoroutineScope
import java.util.Calendar


class ImageRepository(val scope: CoroutineScope, private val dao: ImageDAO) {

    val allImageData = dao.allImages().asLiveData()

    //save new image to room database
    suspend fun saveNewImage(bitmap: Bitmap, fileName: String): Long {
        val timestamp = Calendar.getInstance().time;
        val thumbnailBitmap = createThumbnail(bitmap)
        return dao.addImageData(ImageData(timestamp, fileName, thumbnailBitmap))
    }

    //update image already in room database
    suspend fun updateImage(imageData: ImageData, currentBitmap: Bitmap){
        imageData.timestamp = Calendar.getInstance().time;
        imageData.thumbnail = createThumbnail(currentBitmap)
        dao.updateImageData(imageData)
    }

    //scales bitmap to thumbnail for saving to db for previews
    private fun createThumbnail(drawingBitmap: Bitmap): Bitmap {
        val thumbnailSize = 128;
        return ThumbnailUtils.extractThumbnail(drawingBitmap, thumbnailSize, thumbnailSize);
    }

    //calls the method to remove the image from the database
    suspend fun deleteImage(data : ImageData) {
        dao.deleteImage(data)
    }

    suspend fun getImageById(id: Long): ImageData? {
        return dao.getImageById(id)
    }
}