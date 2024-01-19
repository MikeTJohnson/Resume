package com.example.drawingapp.fragments


import android.annotation.SuppressLint
import android.content.Context
import android.content.pm.ActivityInfo
import android.graphics.Bitmap
import android.hardware.Sensor
import android.hardware.SensorEvent
import android.hardware.SensorEventListener
import android.hardware.SensorManager
import android.os.Build
import android.os.Bundle
import android.util.Log
import android.view.Display
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.view.WindowManager
import androidx.annotation.RequiresApi
import androidx.appcompat.app.AlertDialog
import androidx.core.content.getSystemService
import androidx.fragment.app.Fragment
import androidx.fragment.app.activityViewModels
import androidx.lifecycle.lifecycleScope
import androidx.navigation.fragment.findNavController
import androidx.test.core.app.ApplicationProvider.getApplicationContext
import com.example.drawingapp.DrawingApplication
import com.example.drawingapp.MainActivity
import com.example.drawingapp.R
import com.example.drawingapp.databinding.FragmentDrawingBinding
import com.example.drawingapp.models.DrawingViewModel
import com.example.drawingapp.models.DrawingViewModelFactory
import com.example.drawingapp.models.StrokeStyle
import com.example.drawingapp.utils.FileShare
import com.example.drawingapp.views.DrawingView
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import java.io.ByteArrayOutputStream
import java.io.File


class DrawingFragment : Fragment() {
    private lateinit var drawingView: DrawingView

    private lateinit var sensorManager: SensorManager
    private lateinit var gravity: Sensor
    private lateinit var main: MainActivity
    private lateinit var listener: SensorEventListener
    private lateinit var display: Display
    private var sensorOn = false;
    private var locked = false;

    private val viewModel: DrawingViewModel by activityViewModels{
        DrawingViewModelFactory(
            (requireActivity().application as DrawingApplication).imageRepo,
            resources.displayMetrics.widthPixels,
        )
    }
    //
    @RequiresApi(Build.VERSION_CODES.R)
    @SuppressLint("RestrictedApi")
    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        main = requireActivity() as MainActivity
        display = main.display!!
        sensorManager = main.getSystemService(Context.SENSOR_SERVICE) as SensorManager
        // TODO: Ensure the device has a gravity sensor
        gravity = sensorManager.getDefaultSensor(Sensor.TYPE_GRAVITY)
        listener = object: SensorEventListener {
            override fun onSensorChanged(event: SensorEvent?){
                if( event != null){
                    viewModel.updateGravityData(mapSensors(event.values.copyOf()))
                }
            }
            override fun onAccuracyChanged(sensor: Sensor?, accuracy: Int) {
                // not implemented, unused
            }
        }

        val binding = FragmentDrawingBinding.inflate(inflater)
        val fragmentContainer = binding.fragmentContainerView
        val selectionFragment = SelectionFragment()
        // Add the SelectionFragment to the fragment container
        childFragmentManager.beginTransaction()
            .add(fragmentContainer.id, selectionFragment)
            .commit()
        drawingView = binding.drawingView

        // Set up the click listener for save button
        binding.saveButton.setOnClickListener{
            saveImage()
        }

        //set up the click listener for the delete button
        binding.deleteButton.setOnClickListener{
            showDeleteConfirmAlert()
        }

        //click listener for back button
        binding.backButton.setOnClickListener{
            showBackConfirmAlert()
        }

        //click listener for share button
        binding.shareButton.setOnClickListener{
            drawingView.getImageData()?.let { it1 -> FileShare.shareImage(requireContext(), it1) }
        }

        viewModel.imageData.observe(viewLifecycleOwner) { imageData ->
            drawingView.setImageData(imageData)
        }

        viewModel.bitmapFileName.observe(viewLifecycleOwner) { fileName ->
            drawingView.setFileName(fileName)
        }

        // Set the bitmap in the DrawingView when it's updated in the ViewModel
        viewModel.gravityData.observe(viewLifecycleOwner) {
            drawingView.updateMarbleCoords(it)
            Log.i("GravityData", it.toString())
        }

        // Check if there's a saved bitmap in the ViewModel and set it in the DrawingView
        viewModel.bitmap.observe(viewLifecycleOwner) { bitmap ->
            drawingView.setBitmap(bitmap)
        }

        //set the pen color to the value saved in the view model, also update image in the selectionFrag
        viewModel.penColor.observe(viewLifecycleOwner) {paint ->
            drawingView.setPenColor(paint.color)
            drawingView.setStrokeWidth(paint.strokeWidth)
            selectionFragment.setColorSelection(paint.color)
            selectionFragment.setSliderPosition(paint.strokeWidth)
        }

        //set the stroke style to the value saved in the view model
        viewModel.currentStrokeStyle.observe(viewLifecycleOwner) {style ->
            drawingView.setStrokeStyle(style)
            selectionFragment.setButtonStyle(style)
            if(style == StrokeStyle.MARBLE){
                sensorManager.registerListener(listener, gravity, SensorManager.SENSOR_DELAY_GAME)
                sensorOn = true
                requireActivity().requestedOrientation = ActivityInfo.SCREEN_ORIENTATION_LOCKED
                locked = true
            } else {
                sensorManager.unregisterListener(listener)
                sensorOn = false
                requireActivity().requestedOrientation = ActivityInfo.SCREEN_ORIENTATION_UNSPECIFIED
                locked = false
            }
        }

        return binding.root
    }

    override fun onPause() {
        super.onPause()
        // Save the current drawing to the ViewModel
        viewModel.updateBitmap(drawingView.getBitmap())
        if(sensorOn){
            sensorManager.unregisterListener(listener)
        }
        if(locked){
            requireActivity().requestedOrientation = ActivityInfo.SCREEN_ORIENTATION_UNSPECIFIED
        }
    }

    override fun onResume() {
        super.onResume()
        // Restore the drawing from the ViewModel
        viewModel.bitmap.value?.let { drawingView.setBitmap(it) }
        if(sensorOn){
            sensorManager.registerListener(listener, gravity, SensorManager.SENSOR_DELAY_GAME)
        }
        if(locked) {
            requireActivity().requestedOrientation = ActivityInfo.SCREEN_ORIENTATION_LOCKED
        }
    }

    //Delete confirmation dialog displayed on delete click
    private fun showDeleteConfirmAlert() {
        AlertDialog.Builder(requireActivity())
            .setTitle("Delete entry")
            .setMessage("Are you sure you want to delete this entry?")
            .setPositiveButton("Yes") { _, _ ->
                deleteImage()
            }
            .setNegativeButton("Cancel") { _, _ ->
                // do nothing, android auto closes
            }
            .setIcon(android.R.drawable.ic_dialog_alert)
            .show()
    }

    //Back button confirmation dialog displayed on back click
    private fun showBackConfirmAlert() {
        AlertDialog.Builder(requireActivity())
            .setTitle("Go Back?")
            .setMessage("Discard any unsaved changes?")
            .setPositiveButton("Save") { _, _ ->
                saveImage()
                findNavController().navigate(R.id.action_drawingFragment_to_drawingSelectionFragment)
            }
            .setNegativeButton("Don't Save") { _, _ ->
                findNavController().navigate(R.id.action_drawingFragment_to_drawingSelectionFragment)
            }
            .show()
    }

    private fun mapSensors(input: FloatArray) : FloatArray {
        val output = FloatArray(3)
        when (display.rotation) {
            0 -> {
                output[0] = input[0]
                output[1] = input[1]
            }
            1 -> {
                output[0] = -input[1]
                output[1] = -input[0]
            }
            2 -> {
                output[0] = input[0]
                output[1] = input[1]
            }
            3 -> {
                output[0] = -input[1]
                output[1] = -input[0]
            }
        }
        return output
    }

    //Delete image on confirmation
    private fun deleteImage() {
        lifecycleScope.launch {
            try {
                val imageData = drawingView.getImageData()
                val currentFileName = drawingView.getFileName()
                val path = context?.filesDir?.absolutePath
                val file = File("$path/$currentFileName")
                if (imageData != null) {
                    viewModel.deleteImage(imageData)
                    file.delete()
                }
                findNavController().navigate(R.id.drawingSelectionFragment)
            }
            catch (e: Exception) {
                Log.e("error", "File delete error")
            }
        }
    }

    //save image to database and app files
    private fun saveImage() {
        lifecycleScope.launch{
            val imageData = drawingView.getImageData()
            val currentBitmap = drawingView.getBitmap()
            val currentFileName = drawingView.getFileName()
            val bos = ByteArrayOutputStream()
            currentBitmap.compress(Bitmap.CompressFormat.PNG, 100, bos)
            context?.openFileOutput(currentFileName, Context.MODE_PRIVATE).use {
                it?.write(bos.toByteArray())
            }
            withContext(Dispatchers.IO) {
                bos.close()
            }
            if(imageData == null){
                viewModel.saveNewImageData(currentBitmap, currentFileName)
            } else {
                viewModel.updateDatabaseImageData(imageData, currentBitmap)
            }
        }
    }
}