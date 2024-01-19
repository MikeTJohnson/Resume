package com.example.drawingapp.fragments

import android.graphics.Bitmap
import android.os.Bundle
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
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.ArrowBack
import androidx.compose.material.icons.filled.Refresh
import androidx.compose.material3.CenterAlignedTopAppBar
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.material3.TopAppBarDefaults
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.livedata.observeAsState
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
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
import androidx.core.graphics.drawable.toBitmap
import androidx.fragment.app.Fragment
import androidx.fragment.app.activityViewModels
import androidx.lifecycle.LifecycleObserver
import androidx.navigation.fragment.NavHostFragment.Companion.findNavController
import coil.imageLoader
import coil.request.CachePolicy
import coil.request.ImageRequest
import com.example.drawingapp.DrawingApplication
import com.example.drawingapp.R
import com.example.drawingapp.data.Drawing
import com.example.drawingapp.models.DrawingViewModel
import com.example.drawingapp.models.DrawingViewModelFactory
import com.example.drawingapp.models.SharingViewModel
import com.example.drawingapp.models.SharingViewModelFactory
import com.example.drawingapp.utils.IdToken.Companion.getIdToken
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext

class ImportFragment : Fragment(), LifecycleObserver {

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

    override fun onResume() {
        super.onResume()
        val scope = CoroutineScope(Dispatchers.IO)
        scope.launch { sharingViewModel.requestDrawingList(requireContext()) }
    }

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
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
        Scaffold(
            topBar = {
                CenterAlignedTopAppBar(
                    colors = TopAppBarDefaults.centerAlignedTopAppBarColors(
                        containerColor = MaterialTheme.colorScheme.primaryContainer,
                        titleContentColor = MaterialTheme.colorScheme.primary,
                    ),
                    title = {
                        Text("Import Drawing")
                    },
                    navigationIcon = {
                        IconButton(
                            onClick = {
                                navController.navigate(R.id.drawingSelectionFragment)
                            },
                            content = {
                                Icon(
                                    imageVector = Icons.Default.ArrowBack,
                                    contentDescription = "Back",
                                    tint = MaterialTheme.colorScheme.primary
                                )
                            }
                        )
                    },
                    actions = {
                        IconButton(
                            onClick = {
                                navController.navigate(R.id.importFragment)
                            },
                            content = {
                                Icon(
                                    imageVector = Icons.Default.Refresh,
                                    contentDescription = "Refresh",
                                    tint = MaterialTheme.colorScheme.primary
                                )
                            }
                        )
                    }
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
        val images = sharingViewModel.importImageData.observeAsState()
        LazyVerticalGrid(
            columns = GridCells.Adaptive(180.dp),
            contentPadding = PaddingValues(8.dp)
        ) {
            itemsIndexed(images.value ?: listOf()) { _, image ->
                DrawingSelector(drawingData = image)
            }
        }
    }

    @Composable
    fun DrawingSelector(drawingData: Drawing) {
        // image for use in displaying the drawing and to be passed to the drawing fragment
        var loadedImage: Bitmap? by remember { mutableStateOf(null) }

        LaunchedEffect(drawingData) {
            val token = getIdToken() ?: return@LaunchedEffect
            // https://github.com/coil-kt/coil/tree/main
            val imageRequest = ImageRequest.Builder(requireContext())
                .data("http://10.0.2.2:8080/drawing/${drawingData.filePath}")
                .addHeader("Authorization", token)
                .networkCachePolicy(CachePolicy.ENABLED)
                .diskCachePolicy(CachePolicy.ENABLED)
                .memoryCachePolicy(CachePolicy.ENABLED)
                .build()
            val result = withContext(Dispatchers.IO) {
                imageRequest.context.imageLoader.execute(imageRequest).drawable!!.toBitmap()
            }
            loadedImage = result
        }
        Box(
            modifier = Modifier
                .width(256.dp) // Set the width of the column as needed
                .clickable(onClick = {
                    if (loadedImage == null) return@clickable // No click if image not loaded
                    viewModel.importNewDrawing(loadedImage!!)
                    navController.navigate(R.id.drawingFragment)

                })
        ) {
            if (loadedImage != null) {
                Image(
                    bitmap = loadedImage!!.asImageBitmap(),
                    contentDescription = "Drawing thumbnail for ${drawingData.filePath}",
                    contentScale = ContentScale.Fit,
                    modifier = Modifier
                        .fillMaxSize()
                        .border(BorderStroke(1.dp, Color.Black))
                        .aspectRatio(1f),
                )
            } else {
                Image(
                    painter = painterResource(id = com.firebase.ui.auth.R.drawable.mtrl_ic_error),
                    contentDescription = "Error loading image for ${drawingData.filePath}",
                    contentScale = ContentScale.Fit,
                    modifier = Modifier
                        .fillMaxSize()
                        .border(BorderStroke(1.dp, Color.Black))
                        .aspectRatio(1f)
                )
            }
            // Username center top of image
            Text(
                text = drawingData.userName,
                fontSize = 16.sp,
                color = MaterialTheme.colorScheme.primary,
                modifier = Modifier
                    .padding(8.dp)
                    .fillMaxWidth()
                    .wrapContentWidth(Alignment.CenterHorizontally),
            )
            if (drawingData.canDelete) {
                IconButton(
                    onClick = {
                        CoroutineScope(Dispatchers.IO).launch {
                            sharingViewModel.deleteDrawing(requireContext() ,drawingData)
                        }
                        // On UI thread...
                        navController.navigate(R.id.importFragment) // Refresh the list
                    }
                ) {
                    Icon(
                        painter = painterResource(id = androidx.appcompat.R.drawable.abc_ic_clear_material),
                        contentDescription = "Delete drawing button for ${drawingData.filePath}",
                        tint = Color.Red
                    )
                }
            }
        }
    }
}