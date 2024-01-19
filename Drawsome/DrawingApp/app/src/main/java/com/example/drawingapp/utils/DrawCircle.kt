package com.example.drawingapp.utils

import android.graphics.Canvas
import android.graphics.Paint
import android.view.MotionEvent
import com.example.drawingapp.views.DrawingView

class DrawCircle (
    private val drawingView: DrawingView,
    private val canvas: Canvas,
    private val paint: Paint
) {
    fun onTouchEvent(event: MotionEvent): Boolean {
        val x = event.x
        val y = event.y

        when (event.action) {
            MotionEvent.ACTION_DOWN -> {
                // Draw a circle when the user touches the screen
                canvas.drawCircle(x, y, paint.strokeWidth / 2, paint)
                drawingView.invalidate()
            }
            MotionEvent.ACTION_MOVE -> {
                // Draw more circles while the user moves their finger
                canvas.drawCircle(x, y, paint.strokeWidth / 2, paint)
                drawingView.invalidate()
            }
        }
        return true
    }
}
