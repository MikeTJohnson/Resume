package com.example.drawingapp

import android.content.Context
import android.graphics.Bitmap
import androidx.lifecycle.asLiveData
import androidx.lifecycle.testing.TestLifecycleOwner
import androidx.room.Room
import androidx.test.core.app.ApplicationProvider
import androidx.test.ext.junit.runners.AndroidJUnit4
import com.example.drawingapp.data.ImageDAO
import com.example.drawingapp.data.ImageData
import com.example.drawingapp.data.ImageDatabase
import com.example.drawingapp.data.ImageRepository
import com.example.drawingapp.models.DrawingViewModel
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.SupervisorJob
import kotlinx.coroutines.runBlocking
import kotlinx.coroutines.withContext
import org.junit.After
import org.junit.Assert
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import java.io.IOException
import java.util.Calendar


@RunWith(AndroidJUnit4::class)
class DaoTest {

    private lateinit var db: ImageDatabase
    val scope = CoroutineScope(SupervisorJob())
    private lateinit var repo: ImageRepository
    private lateinit var vm: DrawingViewModel
    private lateinit var dao: ImageDAO

    @Before
    fun createDb() {
        val context = ApplicationProvider.getApplicationContext<Context>()
        db = Room.inMemoryDatabaseBuilder(
            context, ImageDatabase::class.java).build()
        dao = db.imageDao()
        repo = ImageRepository(scope, dao)
        vm = DrawingViewModel(repo, 1200)
    }

    @After
    @Throws(IOException::class)
    fun closeDb() {
        db.close()
    }


    @Test
    @Throws(Exception::class)
    fun testAddImageData_daoTest(){
        runBlocking {
            for(i in 1..100){
                val newBitmap = Bitmap.createBitmap(128, 128, Bitmap.Config.ARGB_8888)
                val timestamp = Calendar.getInstance().time;
                val fileName = "test${i}.png"
                val imageData = ImageData(timestamp, fileName, newBitmap)
                val insertedNewId = dao.addImageData(imageData)
                Assert.assertEquals(i.toLong(), insertedNewId)
                Assert.assertEquals(fileName, dao.getImageById(insertedNewId).fileName)
                Assert.assertEquals(timestamp, dao.getImageById(insertedNewId).timestamp)
            }
        }
    }

    @Test
    fun updateImageDataTest_daoTest(){
        runBlocking {
            val ids = mutableListOf<Long>()
            //insert values make sure they're good
            for(i in 1..100){
                val newBitmap = Bitmap.createBitmap(128, 128, Bitmap.Config.ARGB_8888)
                val timestamp = Calendar.getInstance().time
                val fileName = "test${i}.png"
                val imageData = ImageData(timestamp, fileName, newBitmap)
                ids.add(dao.addImageData(imageData))
                Assert.assertEquals(i.toLong(), ids[i-1])
                Assert.assertEquals(fileName, dao.getImageById(ids[i-1]).fileName)
                Assert.assertEquals(timestamp, dao.getImageById(ids[i-1]).timestamp)
            }
            //update to new values and make sure they changed
            for(id in ids){
                val beforeImageData = dao.getImageById(id)
                val beforeTimestamp = beforeImageData.timestamp
                val beforeFileName = beforeImageData.fileName
                val newTimeStamp = Calendar.getInstance().time
                beforeImageData.timestamp = newTimeStamp
                dao.updateImageData(beforeImageData)
                Assert.assertNotEquals(beforeTimestamp, dao.getImageById(id).timestamp)
                Assert.assertEquals(dao.getImageById(id).timestamp, newTimeStamp)
                val newFileName = "testNewFileName${id}.png"
                beforeImageData.fileName = newFileName
                dao.updateImageData(beforeImageData)
                Assert.assertNotEquals(beforeFileName, dao.getImageById(id).fileName)
                Assert.assertEquals(dao.getImageById(id).fileName, newFileName)
            }
        }
    }

    @Test
    fun removeImageDataTest_daoTest(){
        runBlocking {
            val lifecycleOwner = TestLifecycleOwner()
            for(i in 1..100){
                val newBitmap = Bitmap.createBitmap(128, 128, Bitmap.Config.ARGB_8888)
                val timestamp = Calendar.getInstance().time;
                val fileName = "test${i}.png"
                val imageData = ImageData(timestamp, fileName, newBitmap)
                val insertedNewId = dao.addImageData(imageData)
                Assert.assertEquals(i.toLong(), insertedNewId)
                Assert.assertEquals(fileName, dao.getImageById(insertedNewId).fileName)
                Assert.assertEquals(timestamp, dao.getImageById(insertedNewId).timestamp)

                lifecycleOwner.run{
                    val allImages = dao.allImages().asLiveData()
                    withContext(Dispatchers.Main) {
                        dao.allImages().asLiveData()
                    }
                }
                Assert.assertNotNull(dao.getImageById(insertedNewId))
                if(i%5 == 0 || i%2 == 0){
                    dao.deleteImage(dao.getImageById(insertedNewId))
                    Assert.assertNull(dao.getImageById(insertedNewId))
                }

            }
        }
    }
}