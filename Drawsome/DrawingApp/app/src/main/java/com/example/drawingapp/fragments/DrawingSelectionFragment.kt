package com.example.drawingapp.fragments

import android.graphics.BitmapFactory
import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.compose.foundation.BorderStroke
import androidx.compose.foundation.Image
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.aspectRatio
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.layout.wrapContentWidth
import androidx.compose.foundation.lazy.grid.GridCells
import androidx.compose.foundation.lazy.grid.LazyVerticalGrid
import androidx.compose.foundation.lazy.grid.itemsIndexed
import androidx.compose.material3.CenterAlignedTopAppBar
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.material3.TopAppBarDefaults
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.livedata.observeAsState
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.asImageBitmap
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.platform.ComposeView
import androidx.compose.ui.platform.ViewCompositionStrategy
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.fragment.app.Fragment
import androidx.fragment.app.activityViewModels
import androidx.navigation.fragment.NavHostFragment.Companion.findNavController
import com.example.drawingapp.DrawingApplication
import com.example.drawingapp.R
import com.example.drawingapp.data.ImageData
import com.example.drawingapp.models.DrawingViewModel
import com.example.drawingapp.models.DrawingViewModelFactory
import com.example.drawingapp.models.SharingViewModel
import com.example.drawingapp.models.SharingViewModelFactory
import com.example.drawingapp.utils.FileShare
import com.firebase.ui.auth.AuthUI
import com.firebase.ui.auth.FirebaseAuthUIActivityResultContract
import com.google.firebase.auth.FirebaseAuth
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import java.io.File


class DrawingSelectionFragment : Fragment() {

    private val viewModel: DrawingViewModel by activityViewModels {
        DrawingViewModelFactory(
            (requireActivity().application as DrawingApplication).imageRepo,
            resources.displayMetrics.widthPixels,
        )
    }
    private val sharingViewModel: SharingViewModel by activityViewModels{
        SharingViewModelFactory(
            (requireActivity().application as DrawingApplication).sharingRepo
        )
    }
    private lateinit var navController: androidx.navigation.NavController

    // https://github.com/firebase/FirebaseUI-Android/blob/master/auth/README.md#configuration
    private val signInLauncher = registerForActivityResult(
        FirebaseAuthUIActivityResultContract()
    ) { result ->
        result.idpResponse
        if (result.resultCode == android.app.Activity.RESULT_OK) {
            // Successfully signed in
            val user = FirebaseAuth.getInstance().currentUser
            Log.i("user", user.toString())
            navController.navigate(R.id.drawingSelectionFragment)
        } else {
            Log.i("user", "failed")
        }
    }
    override fun onResume() {
        super.onResume()
        val scope = CoroutineScope(Dispatchers.IO)
        scope.launch { sharingViewModel.updateServerStatus(requireContext()) }
    }

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?,
    ): View {
        navController = findNavController(this)
        return ComposeView(requireContext()).apply {
            setViewCompositionStrategy(ViewCompositionStrategy.DisposeOnViewTreeLifecycleDestroyed)
            setContent {
                DrawingSelectionScreen()
                val toast by sharingViewModel.toastMessage.collectAsState("")

                if (toast != "") {
                    LaunchedEffect(toast) {
                        Toast.makeText(requireContext(), toast, Toast.LENGTH_SHORT).show()
                        sharingViewModel.setToastMessage("")
                    }

                }
            }
        }
    }

    @OptIn(ExperimentalMaterial3Api::class)
    @Composable
    fun DrawingSelectionScreen() {
        sharingViewModel.updateLoggedIn(FirebaseAuth.getInstance().currentUser != null)
        val loggedIn = sharingViewModel.loggedIn.observeAsState().value!!

        Scaffold(
            topBar = {
                CenterAlignedTopAppBar(
                    colors = TopAppBarDefaults.centerAlignedTopAppBarColors(
                        containerColor = MaterialTheme.colorScheme.primaryContainer,
                        titleContentColor = MaterialTheme.colorScheme.primary,
                    ),
                    title = {
                        Text("My Drawings")
                    },
                    actions = {
                        if (loggedIn) ImportButton()
                        //Button for new drawing
                        NewDrawingButton()
                    },
                    navigationIcon = {
                        TextButton(
                            onClick = {
                                if (loggedIn) {
                                    FirebaseAuth.getInstance().signOut()
                                    navController.navigate(R.id.drawingSelectionFragment)
                                } else {
                                    val signInIntent = AuthUI.getInstance()
                                        .createSignInIntentBuilder()
                                        .build()

                                    signInLauncher.launch(signInIntent)
                                }

                            },
                            content = {
                                if (loggedIn){
                                    Text(text = "Sign Out")
                                }
                                else Text(text = "Sign In")
                            }
                        )
                    },
                )
            },
        ) { paddingValues ->
            Surface(
                modifier = Modifier
                    .padding(paddingValues)
                    .fillMaxSize(),
                color = Color(0xFF8dcafe),
            ) {
                DrawingsList()
            }
        }
    }

    @Composable
    fun DrawingsList() {
        //potentially change to columns = GridCells.Adaptive(minSize = 256.dp) to adjust for diff devices
        val images = viewModel.allImages.observeAsState()
        LazyVerticalGrid(
            columns = GridCells.Adaptive(180.dp),
            contentPadding = PaddingValues(8.dp)
        ) {
            itemsIndexed(images.value ?: listOf()) { _, image ->
                DrawingSelector(imageData = image)
            }
        }
    }

    @Composable
    fun DrawingSelector(imageData: ImageData) {
        val scope = rememberCoroutineScope()
        val loggedIn = sharingViewModel.loggedIn.observeAsState().value!!
        Box(
            modifier = Modifier
                .width(256.dp) // Set the width of the column as needed
                .clickable(onClick = {
                    // Load file png to get bitmap for drawing
                    val file = File(context?.filesDir, imageData.fileName).readBytes()
                    val bmp = BitmapFactory.decodeByteArray(file, 0, file.size)
                    viewModel.clearBitmap(bmp)
                    viewModel.updateBitmapFileName(imageData.fileName)
                    viewModel.updateImageData(imageData)
                    navController.navigate(R.id.drawingFragment)
                })
        ) {
            Image(
                bitmap = imageData.thumbnail.asImageBitmap(),
                contentDescription = "Drawing thumbnail for ${imageData.timestamp}",
                contentScale = ContentScale.Fit,
                modifier = Modifier
                    .fillMaxSize()
                    .border(BorderStroke(1.dp, Color.Black))
                    .aspectRatio(1f)
            )

            // Text below the image
            Text(
                text = "${imageData.timestamp}",
                fontSize = 16.sp,
                color = MaterialTheme.colorScheme.primary,
                modifier = Modifier
                    .padding(8.dp)
                    .fillMaxWidth()
                    .wrapContentWidth(Alignment.CenterHorizontally),
            )

            // Button in the bottom right corner
            IconButton(
                onClick = {
                    FileShare.shareImage(requireContext(), imageData)
                },
                modifier = Modifier
                    .align(Alignment.BottomEnd)
                    .padding(8.dp)
            ) {
                Icon(
                    painter = painterResource(id = androidx.appcompat.R.drawable.abc_ic_menu_share_mtrl_alpha),
                    contentDescription = "Share for ${imageData.timestamp}"
                )
            }
            if (loggedIn) {
                val isServerRunning by sharingViewModel.serverRunning.observeAsState(false)
                IconButton(
                    onClick = {
                        if (!isServerRunning) {
                            Toast.makeText(
                                requireContext(),
                                "Server is not running, please try again later",
                                Toast.LENGTH_SHORT
                            ).show()
                        } else {
                            val currentFileName = imageData.fileName
                            val path = context?.filesDir?.absolutePath
                            val file = "$path/$currentFileName"
                            scope.launch { sharingViewModel.uploadToServer(file) }
                        }
                    },
                    modifier = Modifier
                        .align(Alignment.BottomStart)
                        .padding(8.dp)
                ) {
                    Icon(
                        painter = painterResource(id = R.drawable.baseline_upload_24),
                        contentDescription = "upload ${imageData.timestamp}"
                    )
                }
            }
        }
    }


    @Composable
    fun NewDrawingButton() {
        IconButton(
            onClick = {
                viewModel.createNewDrawing()
                navController.navigate(R.id.drawingFragment)
            })
        {
            Icon(
                painter = painterResource(id = R.drawable.baseline_add_photo_alternate_24),
                contentDescription = "New Drawing"
            )
        }
    }

    @Composable
    fun ImportButton() {
        val isServerRunning by sharingViewModel.serverRunning.observeAsState(false)

        IconButton(
            onClick = {
                if (!isServerRunning) {
                    Toast.makeText(
                        requireContext(),
                        "Server is not running, please try again later",
                        Toast.LENGTH_SHORT
                    ).show()
                } else {
                    navController.navigate(R.id.importFragment)
                }
            }
        )
        {
            Icon(
                painter = painterResource(id = R.drawable.baseline_file_download_24),
                contentDescription = "Import Drawing"
            )
        }
    }
}