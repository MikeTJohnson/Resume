package com.example.drawingapp.fragments

import android.annotation.SuppressLint
import android.os.Bundle
import android.os.Handler
import android.os.Looper
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.navigation.fragment.findNavController
import com.example.drawingapp.R
import com.example.drawingapp.databinding.FragmentSplashScreenBinding


@SuppressLint("CustomSplashScreen")
class SplashScreen : Fragment() {
    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        // Inflate the layout for this fragment
        val binding = FragmentSplashScreenBinding.inflate(inflater, container, false)
        binding.run {
            Handler(Looper.getMainLooper())
                .postDelayed({findNavController()
                    .navigate(R.id.action_splashscreen_to_drawingSelectionFragment)
            }, 2000)
        }
        return binding.root
    }
}