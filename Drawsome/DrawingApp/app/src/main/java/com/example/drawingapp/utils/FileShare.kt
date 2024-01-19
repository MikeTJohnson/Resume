package com.example.drawingapp.utils

import android.content.Context
import android.content.Intent
import android.util.Log
import android.widget.Toast
import androidx.core.content.FileProvider
import com.example.drawingapp.data.ImageData
import java.io.File

class FileShare (){
    companion object {
        fun shareImage(context: Context, imageData: ImageData) {
            val file = File(context.filesDir, imageData.fileName)

            if (file.exists()) {
                val uri = FileProvider.getUriForFile(
                    context,
                    "${context.packageName}.provider",
                    file
                )

                val shareIntent = Intent(Intent.ACTION_SEND).apply {
                    type = "image/*" // Set the MIME type of the file
                    putExtra(Intent.EXTRA_STREAM, uri)
                    addFlags(Intent.FLAG_GRANT_READ_URI_PERMISSION) //
                }

                context.startActivity(Intent.createChooser(shareIntent, "Share via"))
            } else {
                Toast.makeText(context, "Failed to Share", Toast.LENGTH_SHORT).show()
                Log.e("DrawingSelectionFragment", "File does not exist")
            }
        }
    }
}