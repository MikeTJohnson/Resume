package com.example.drawingapp.utils

import android.graphics.Canvas
import android.graphics.Paint
import android.view.MotionEvent
import com.example.drawingapp.views.DrawingView

class DrawSquare(
    private val drawingView: DrawingView,
    private val canvas: Canvas,
    private val paint: Paint
) {
    fun onTouchEvent(event: MotionEvent): Boolean {
        val x = event.x
        val y = event.y

        when (event.action) {
            MotionEvent.ACTION_DOWN -> {
                // Draw a square when the user touches the screen
                val halfSize = paint.strokeWidth / 2
                canvas.drawRect(
                    x - halfSize, y - halfSize, x + halfSize, y + halfSize, paint
                )
                drawingView.invalidate()
            }
            MotionEvent.ACTION_MOVE -> {
                // Draw more squares while the user moves their finger
                val halfSize = paint.strokeWidth / 2
                canvas.drawRect(
                    x - halfSize, y - halfSize, x + halfSize, y + halfSize, paint
                )
                drawingView.invalidate()
            }
        }
        return true
    }
}
