package com.example.drawingapp.data

import android.graphics.Bitmap
import android.graphics.BitmapFactory
import androidx.room.ColumnInfo
import androidx.room.Entity
import androidx.room.PrimaryKey
import androidx.room.TypeConverter
import java.io.ByteArrayOutputStream
import java.util.Date


//apparently Room can't handle Date objects directly...
class Converters {
    @TypeConverter
    fun fromTimestamp(value: Long?): Date? {
        return value?.let { Date(it) }
    }

    @TypeConverter
    fun dateToTimestamp(date: Date?): Long? {
        return date?.time
    }

    @TypeConverter
    fun bitmapToByteArray(bitmap: Bitmap?): ByteArray? {
        if(bitmap != null){
            val bos = ByteArrayOutputStream()
            bitmap.compress(Bitmap.CompressFormat.PNG, 100, bos)
            val bytes = bos.toByteArray()
            bos.close()
            return bytes
        }
        return null
    }

    @TypeConverter
    fun blobToBitmap(bitmapData: ByteArray?): Bitmap? {
        if(bitmapData != null) {
            return BitmapFactory.decodeByteArray(bitmapData, 0, bitmapData.size)
        }
        return null
    }
}


//Store string for filename
@Entity(tableName="images")
data class ImageData(var timestamp: Date, var fileName: String, var thumbnail: Bitmap){
    @PrimaryKey(autoGenerate = true)
    var id: Int = 0 //integer primary key for DB
    @ColumnInfo(name = "image", typeAffinity = ColumnInfo.BLOB)
    var image: ByteArray? = null
}
