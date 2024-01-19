package com.example.drawingapp.utils

import android.graphics.Canvas
import android.graphics.Paint
import android.graphics.Path
import android.view.MotionEvent
import com.example.drawingapp.views.DrawingView

class DrawPath(
    private val drawingView: DrawingView,
    private val canvas: Canvas,
    private val path: Path,
    private val paint: Paint
){
    private var lastX: Float = 0f
    private var lastY: Float = 0f

    fun onTouchEvent(event: MotionEvent): Boolean {
        val x = event.x
        val y = event.y

        when (event.action) {
            MotionEvent.ACTION_DOWN -> {
                path.reset()
                path.moveTo(x, y)
                lastX = x
                lastY = y
            }
            MotionEvent.ACTION_MOVE -> {
                path.quadTo(lastX, lastY, (x + lastX) / 2, (y + lastY) / 2)
                lastX = x
                lastY = y
                drawingView.invalidate()
            }
            MotionEvent.ACTION_UP -> {
                path.lineTo(x, y)
                canvas.drawPath(path, paint)
                path.reset()
                drawingView.invalidate()
            }
        }
        return true
    }
}
