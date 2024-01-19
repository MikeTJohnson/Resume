package com.example.drawingapp

import android.content.Context
import android.graphics.Bitmap
import android.graphics.BitmapFactory
import android.graphics.Color
import android.graphics.Paint
import androidx.lifecycle.testing.TestLifecycleOwner
import androidx.room.Room
import androidx.test.core.app.ApplicationProvider
import androidx.test.ext.junit.runners.AndroidJUnit4
import androidx.test.platform.app.InstrumentationRegistry
import com.example.drawingapp.data.ImageDAO
import com.example.drawingapp.data.ImageData
import com.example.drawingapp.data.ImageDatabase
import com.example.drawingapp.data.ImageRepository
import com.example.drawingapp.models.DrawingViewModel
import com.example.drawingapp.models.StrokeStyle
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.SupervisorJob
import kotlinx.coroutines.runBlocking
import kotlinx.coroutines.withContext
import org.junit.After
import org.junit.Assert.*
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import java.io.IOException
import java.util.Date
import java.util.concurrent.CountDownLatch

class DatabaseTests {


    private lateinit var db: ImageDatabase
    val scope = CoroutineScope(SupervisorJob())
    private lateinit var repo: ImageRepository
    private lateinit var vm: DrawingViewModel
    private lateinit var dao: ImageDAO

    @Before
    fun createDb() {
        val context = ApplicationProvider.getApplicationContext<Context>()
        db = Room.inMemoryDatabaseBuilder(
            context, ImageDatabase::class.java
        ).build()
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
    fun stressTestSaveDeleteImage() {
        runBlocking {
            val fileName = "testName"
            val image = ImageData(Date(), fileName, Bitmap.createBitmap(800, 1100, Bitmap.Config.ARGB_8888))
            for (i in 1..10) {
                dao.addImageData(image)
                val check = dao.oneImage()
                assert(check.fileName == image.fileName)
                dao.deleteImage(check)
                val nullCheck = dao.oneImage()
                assertNull(nullCheck)
            }
        }
    }

    @Test
    fun manyUpdatesOneDelete() {
        runBlocking {
            val fileName = "testName"
            val image = ImageData(Date(), fileName, Bitmap.createBitmap(800, 1100, Bitmap.Config.ARGB_8888))
            dao.addImageData(image)
            for (i in 1..10) {
                dao.updateImageData(image)
            }
            val check = dao.oneImage()
            dao.deleteImage(check)
            val nullCheck = dao.oneImage()
            assertNull(nullCheck)
        }
    }

//    @Test
//    fun stressTestSaveDeleteThroughVM() {
//        runBlocking {
//            val lifecycleOwner = TestLifecycleOwner()
//            lifecycleOwner.run {
//                withContext(Dispatchers.Main) {
//                    val fileName = "testName"
//                    val image = Bitmap.createBitmap(800, 1100, Bitmap.Config.ARGB_8888)
//                    val before = vm.allImages.value?.get(0)
//                    runBlocking {
//                        vm.saveNewImageData(image, fileName)
//                    }
//                    val after = vm.allImages.value?.get(0)
//                    assert(before != after)
//                }
//            }
//        }
//    }
}