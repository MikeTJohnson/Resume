package com.example.drawingapp

import android.content.Context
import android.graphics.Bitmap
import android.graphics.BitmapFactory
import android.graphics.Color
import android.graphics.Paint
import android.media.ThumbnailUtils
import androidx.lifecycle.testing.TestLifecycleOwner
import androidx.room.Room
import androidx.test.core.app.ApplicationProvider
import androidx.test.ext.junit.runners.AndroidJUnit4
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

@RunWith(AndroidJUnit4::class)
class ViewModelTest {

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
    fun initializationCorrect(){
        assertEquals(vm.penColor.value?.color, Color.BLACK)
        assertEquals(vm.penColor.value?.style, Paint.Style.STROKE)
        assertEquals(vm.penColor.value?.strokeWidth, 5f)
        assertEquals(vm.currentStrokeStyle.value, StrokeStyle.LINE)
    }

    @Test
    fun updatePenColor_ViewModelTest(){
        runBlocking {
            val lifecycleOwner = TestLifecycleOwner()
            val before = vm.penColor.value?.color
            var callbackFired = false
            lifecycleOwner.run {
                withContext(Dispatchers.Main) {
                    vm.penColor.observe(lifecycleOwner){
                        callbackFired = true
                    }
                    vm.updatePenColor(Color.WHITE)
                    assertTrue(callbackFired)
                    assertNotEquals(before, vm.penColor.value?.color!!)
                }
            }
        }
    }

    @Test
    fun updateStrokeWidth_ViewModelTest(){
        runBlocking {
            val lifecycleOwner = TestLifecycleOwner()
            val before = vm.penColor.value?.strokeWidth
            var callbackFired = false
            lifecycleOwner.run {
                withContext(Dispatchers.Main) {
                    vm.penColor.observe(lifecycleOwner){
                        callbackFired = true
                    }
                    vm.updateStrokeWidth(14.5F)
                    assertTrue(callbackFired)
                    assertNotEquals(before, vm.penColor.value?.strokeWidth!!)
                }
            }
        }
    }

    @Test
    fun updateStrokeStyle_ViewModelTest(){
        runBlocking {
            val lifecycleOwner = TestLifecycleOwner()
            val before = vm.currentStrokeStyle.value
            var callbackFired = false
            lifecycleOwner.run {
                withContext(Dispatchers.Main) {
                    vm.currentStrokeStyle.observe(lifecycleOwner){
                        callbackFired = true
                    }
                    vm.updateStrokeStyle(StrokeStyle.CIRCLE)
                    assertTrue(callbackFired)
                    assertNotEquals(before, vm.currentStrokeStyle.value!!)
                }
            }
        }
    }

    @Test
    fun updateBitmap_ViewModelTest(){
        runBlocking {
            val lifecycleOwner = TestLifecycleOwner()
            val before = vm.bitmap.value
            var callbackFired = false
            lifecycleOwner.run {
                withContext(Dispatchers.Main) {
                    vm.bitmap.observe(lifecycleOwner){
                        callbackFired = true
                    }
                    vm.updateBitmap(Bitmap.createBitmap(800, 1100, Bitmap.Config.ARGB_8888))
                    assertTrue(callbackFired)
                    if (before != null) {
                        assertTrue(before.sameAs(vm.bitmap.value))
                    }
                }
            }
        }
    }

    @Test
    fun testSaveNewImageData() {
        val context: Context = ApplicationProvider.getApplicationContext()
        val inputStream = context.assets.open("apple.png")
        val bitmap = BitmapFactory.decodeStream(inputStream)
        val thumbnail = createThumbnail(bitmap)
        val allImages = vm.allImages
        val lifecycleOwner = TestLifecycleOwner()
        runBlocking {
            lifecycleOwner.run {
                withContext(Dispatchers.Main) {
                    vm.saveNewImageData(bitmap, "first.png")
                    allImages.observe(TestLifecycleOwner()) {
                        assertEquals(allImages.value?.get(0)?.fileName, "first.png")
                        assertEquals(allImages.value?.get(0)?.thumbnail, thumbnail)
                    }
                }
            }
        }
    }

    @Test
    fun writeUserAndReadInTest() {
        runBlocking {
            val fileName = "testName"
            val image = ImageData(Date(), fileName, Bitmap.createBitmap(800, 1100, Bitmap.Config.ARGB_8888))
            dao.addImageData(image)
            val check = dao.oneImage()
            assert(check.fileName == image.fileName)
        }
    }
    private fun createThumbnail(drawingBitmap: Bitmap): Bitmap {
        val thumbnailSize = 128;
        return ThumbnailUtils.extractThumbnail(drawingBitmap, thumbnailSize, thumbnailSize);
    }
}

