package com.example.drawingapp.fragments

import android.content.Context
import android.content.DialogInterface
import android.content.pm.ActivityInfo
import android.graphics.Color
import android.hardware.Sensor
import android.hardware.SensorEvent
import android.hardware.SensorEventListener
import android.hardware.SensorManager
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import androidx.fragment.app.activityViewModels
import androidx.lifecycle.asLiveData
import com.example.drawingapp.DrawingApplication
import com.example.drawingapp.MainActivity
import com.example.drawingapp.R
import com.example.drawingapp.databinding.FragmentSelectionBinding
import com.example.drawingapp.models.DrawingViewModel
import com.example.drawingapp.models.DrawingViewModelFactory
import com.example.drawingapp.models.StrokeStyle
import com.flask.colorpicker.ColorPickerView
import com.flask.colorpicker.builder.ColorPickerDialogBuilder
import com.google.android.material.slider.Slider
import kotlinx.coroutines.channels.awaitClose
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.channelFlow

class SelectionFragment : Fragment() {
    private var currentColor: Int = Color.BLUE
    private val viewModel: DrawingViewModel by activityViewModels{
        DrawingViewModelFactory(
            (requireActivity().application as DrawingApplication).imageRepo,
            resources.displayMetrics.widthPixels,
        )}
    private var _locked = false

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        val binding = FragmentSelectionBinding.inflate(inflater)
        //Makes the slider for changing the stroke width visible or invisible
        binding.strokeWidthActionButton.setOnClickListener {
            val selectedColor = ContextCompat.getColor(requireContext(), R.color.selected_color)
            val unselectedColor = ContextCompat.getColor(requireContext(), R.color.unselected_color)

            val slider = binding.strokeSlider
            if (slider.visibility == View.VISIBLE) {
                slider.visibility = View.INVISIBLE
                binding.strokeWidthActionButton.drawable.setTint(unselectedColor)
            }
            else {
                slider.visibility = View.VISIBLE
                binding.strokeWidthActionButton.drawable.setTint(selectedColor)
            }
        }

        //Pulls up a dialog for selecting the stroke color
        binding.colorActionButton.setOnClickListener {
            ColorPickerDialogBuilder
                .with(context)
                .setTitle("Choose color")
                .initialColor( if(currentColor == Color.BLACK) Color.BLUE else currentColor)
                .wheelType(ColorPickerView.WHEEL_TYPE.FLOWER)
                .density(12)
                .lightnessSliderOnly()
                .setPositiveButton("ok") { _: DialogInterface, selectedColor: Int, _: Array<Int> ->
                    viewModel.updatePenColor(selectedColor)
                }
                .setNegativeButton(
                    "cancel"
                ) { _, _ -> }
                .build()
                .show()
        }

        binding.marbleActionButton.setOnClickListener{
            viewModel.updateStrokeStyle(StrokeStyle.MARBLE)
        }

        // Update Pen Action Button to show selected style
        binding.penActionButton.setOnClickListener {
            viewModel.updateStrokeStyle(StrokeStyle.LINE)
        }

        // Update Circle Action Button to show selected style
        binding.circleActionButton.setOnClickListener {
            viewModel.updateStrokeStyle(StrokeStyle.CIRCLE)
        }

        // Update Square Action Button to show selected style
        binding.squareActionButton.setOnClickListener {
            viewModel.updateStrokeStyle(StrokeStyle.SQUARE)
        }

        //set the slider to handle changes and update the stroke width in the view model
        binding.strokeSlider.visibility = View.INVISIBLE
        binding.strokeSlider.addOnSliderTouchListener(object : Slider.OnSliderTouchListener {
            override fun onStartTrackingTouch(slider: Slider) {}

            override fun onStopTrackingTouch(slider: Slider) {
                viewModel.updateStrokeWidth(slider.value)
            }
        })
        return binding.root
    }

    fun setColorSelection(color: Int){
        val binding = FragmentSelectionBinding.bind(requireView())
        binding.colorActionButton.drawable.setTint(color)
        currentColor = color
    }

    fun setSliderPosition(strokeWidth: Float){
        val binding = FragmentSelectionBinding.bind(requireView())
        binding.strokeSlider.value = strokeWidth
    }

    fun setButtonStyle(style: StrokeStyle?) {
        val selectedColor = ContextCompat.getColor(requireContext(), R.color.selected_color)
        val unselectedColor = ContextCompat.getColor(requireContext(), R.color.unselected_color)

        val binding = FragmentSelectionBinding.bind(requireView())

        when (style ?: StrokeStyle.LINE) {
            StrokeStyle.LINE -> {
                binding.penActionButton.drawable?.setTint(selectedColor)
                binding.circleActionButton.drawable?.setTint(unselectedColor)
                binding.squareActionButton.drawable?.setTint(unselectedColor)
                binding.marbleActionButton.drawable?.setTint(unselectedColor)
            }

            StrokeStyle.CIRCLE -> {
                binding.penActionButton.drawable?.setTint(unselectedColor)
                binding.circleActionButton.drawable?.setTint(selectedColor)
                binding.squareActionButton.drawable?.setTint(unselectedColor)
                binding.marbleActionButton.drawable?.setTint(unselectedColor)
            }

            StrokeStyle.SQUARE -> {
                binding.penActionButton.drawable?.setTint(unselectedColor)
                binding.circleActionButton.drawable?.setTint(unselectedColor)
                binding.squareActionButton.drawable?.setTint(selectedColor)
                binding.marbleActionButton.drawable?.setTint(unselectedColor)
            }

            StrokeStyle.MARBLE -> {
                binding.penActionButton.drawable?.setTint(unselectedColor)
                binding.circleActionButton.drawable?.setTint(unselectedColor)
                binding.squareActionButton.drawable?.setTint(unselectedColor)
                binding.marbleActionButton.drawable?.setTint(selectedColor)
            }
        }
    }
}
