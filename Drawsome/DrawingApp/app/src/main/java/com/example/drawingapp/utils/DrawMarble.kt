
package com.example.drawingapp.utils

import android.graphics.Canvas
import android.graphics.Paint
import com.example.drawingapp.views.DrawingView

class DrawMarble (
    private val drawingView: DrawingView,
    private val canvas: Canvas,
    private val paint: Paint,
    private var x: Float,
    private var y: Float,
) {
    private val xMax = x * 2.0f
    private val yMax = y * 2.0f

    fun drawCircle(gravity: FloatArray){
        x = (x + (-1 *gravity[0]))
            .coerceAtLeast(0f + paint.strokeWidth / 2)
            .coerceAtMost(xMax - paint.strokeWidth / 2)

        y = (y + gravity[1])
            .coerceAtLeast(0f + paint.strokeWidth / 2)
            .coerceAtMost(yMax - paint.strokeWidth / 2)

        canvas.drawCircle(x, y, paint.strokeWidth / 2, paint)
        drawingView.invalidate()
    }
}
