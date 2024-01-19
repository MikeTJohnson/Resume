package com.example.drawingapp.data

import android.content.Context
import androidx.room.Dao
import androidx.room.Database
import androidx.room.Delete
import androidx.room.Insert
import androidx.room.OnConflictStrategy
import androidx.room.Query
import androidx.room.Room
import androidx.room.RoomDatabase
import androidx.room.TypeConverters
import androidx.room.Update
import kotlinx.coroutines.flow.Flow

@Database(entities = [ImageData::class], version = 1, exportSchema = false)
@TypeConverters(Converters::class)
abstract class ImageDatabase: RoomDatabase(){
    abstract fun imageDao(): ImageDAO

    //Make singleton so only one DB opens at same time
    companion object {
        @Volatile
        private var INSTANCE: ImageDatabase? = null

        fun getDatabase(context: Context): ImageDatabase {
            //if instance is not null, return it, otherwise create DB
            return INSTANCE ?: synchronized(this) {
                val instance = Room.databaseBuilder(
                    context.applicationContext,
                    ImageDatabase::class.java,
                    "image_database"
                ).build()
                INSTANCE = instance
                return instance
            }
        }
    }
}


//where we can write our DB queries
@Dao
interface ImageDAO {
    @Insert(onConflict = OnConflictStrategy.REPLACE)
    suspend fun addImageData(data: ImageData) : Long

    @Update
    suspend fun updateImageData(data: ImageData)

    @Query("SELECT * from images ORDER BY timestamp DESC")
    fun allImages() : Flow<List<ImageData>>

    @Delete
    suspend fun deleteImage(data : ImageData)

    @Query("SELECT * from images ORDER BY timestamp DESC LIMIT 1")
    fun oneImage() : ImageData
    @Query("SELECT * from images where id = :id")
    suspend fun getImageById(id: Long) : ImageData
}