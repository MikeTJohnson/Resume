package com.example.drawingapp.views

import android.annotation.SuppressLint
import android.content.Context
import android.graphics.Bitmap
import android.graphics.Canvas
import android.graphics.Color
import android.graphics.Paint
import android.graphics.Path
import android.util.AttributeSet
import android.view.MotionEvent
import android.view.View
import com.example.drawingapp.data.ImageData
import com.example.drawingapp.models.StrokeStyle
import com.example.drawingapp.utils.DrawCircle
import com.example.drawingapp.utils.DrawMarble
import com.example.drawingapp.utils.DrawPath
import com.example.drawingapp.utils.DrawSquare

class DrawingView(context: Context, attrs: AttributeSet) : View(context, attrs) {
    private val screenWidth = resources.displayMetrics.widthPixels
    private val screenHeight = resources.displayMetrics.heightPixels
    private val minDimension = minOf(screenWidth, screenHeight)
    private var imageData: ImageData? = null
    private var bitmap: Bitmap = Bitmap.createBitmap(
        minDimension,
        minDimension,
        Bitmap.Config.ARGB_8888
    )

    private val bitmapCanvas = Canvas(bitmap)
    private val paint = Paint().apply {
        color = Color.BLACK
        style = Paint.Style.STROKE
        strokeWidth = 5f
    }
    private var strokeStyle: StrokeStyle = StrokeStyle.LINE
    private var bitmapFileName: String = ""

    init {
        bitmapCanvas.drawColor(Color.WHITE)
    }

    private var path = Path()
    private val drawPath = DrawPath(this, bitmapCanvas, path, paint)
    private val drawCircle = DrawCircle(this, bitmapCanvas, paint)
    private val drawSquare = DrawSquare(this, bitmapCanvas, paint)
    private val drawMarble = DrawMarble(this, bitmapCanvas, paint,
        (minDimension / 2.0f), (minDimension / 2.0f))

    @SuppressLint("ClickableViewAccessibility")
    override fun onTouchEvent(event: MotionEvent): Boolean {
        return when (strokeStyle) {
            StrokeStyle.CIRCLE -> {
                drawCircle.onTouchEvent(event)
            }

            StrokeStyle.LINE -> {
                drawPath.onTouchEvent(event)
            }

            StrokeStyle.SQUARE -> {
                drawSquare.onTouchEvent(event)
            }

            StrokeStyle.MARBLE -> {
                return false
            }
        }
    }


    override fun onDraw(canvas: Canvas) {
        super.onDraw(canvas)
        canvas.drawBitmap(bitmap, 0f, 0f, paint)
        if (strokeStyle == StrokeStyle.LINE) {
            canvas.drawPath(path, paint) /* Needed for DrawPath */
        }
    }

    // Function to set the bitmap from primary fragment container
    fun setBitmap(bitmap: Bitmap) {
        this.bitmap = bitmap
        bitmapCanvas.setBitmap(bitmap)
        invalidate()
    }

    // Function to get the current bitmap
    fun getBitmap(): Bitmap {
        return bitmap
    }

    //change pen stroke width after slider selection
    fun setStrokeWidth(size : Float) {
        paint.strokeWidth = size
        invalidate()
    }

    //function to set the pen color from the view model
    fun setPenColor(color: Int) {
        paint.color = color
        invalidate()
    }

    //function to set the stroke style from the view model
    fun setStrokeStyle(style: StrokeStyle) {
        strokeStyle = style
        invalidate()
    }

    //set filename for bitmap from view model
    fun setFileName(fileName: String){
        this.bitmapFileName = fileName
    }

    //get filename for saving
    fun getFileName() : String {
        return bitmapFileName
    }

    fun setImageData(imageData: ImageData?) {
        this.imageData = imageData
    }

    fun getImageData() : ImageData? {
        return imageData
    }

    fun updateMarbleCoords(it: FloatArray?) {
        if (it != null && strokeStyle == StrokeStyle.MARBLE) {
            drawMarble.drawCircle(it)
        }
    }

}






