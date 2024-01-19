package com.example.drawingapp

import android.content.Context
import android.graphics.BitmapFactory
import androidx.compose.ui.test.*
import androidx.compose.ui.test.junit4.createComposeRule
import androidx.test.core.app.ApplicationProvider
import androidx.test.ext.junit.runners.AndroidJUnit4
import com.example.drawingapp.data.ImageData
import com.example.drawingapp.fragments.DrawingSelectionFragment
import org.junit.Assert.*
import org.junit.Rule
import org.junit.Test
import org.junit.runner.RunWith
import java.util.Date

@RunWith(AndroidJUnit4::class)
class DrawingSelectionTest {
    private val context: Context = ApplicationProvider.getApplicationContext()

//    private lateinit var navController: NavController

    @get:Rule
    val composeTestRule = createComposeRule()
    @Test
    fun testNewDrawingButton() {
        composeTestRule.setContent { DrawingSelectionFragment().NewDrawingButton() }
        composeTestRule.onNodeWithContentDescription("New Drawing").assertIsDisplayed()
        composeTestRule.onNodeWithContentDescription("New Drawing").assertHasClickAction()
    }

    @Test
    fun testDrawingSelector() {
        val inputStream = context.assets.open("apple.png")
        val bitmap = BitmapFactory.decodeStream(inputStream)
        val imageData = ImageData(
            fileName = "test.png",
            timestamp = Date(),
            thumbnail = bitmap
        )
        composeTestRule.setContent {
            DrawingSelectionFragment().DrawingSelector(imageData = imageData)
        }
        composeTestRule.onNodeWithContentDescription(
            "Drawing thumbnail for ${imageData.timestamp}").assertIsDisplayed()
        composeTestRule.onNodeWithContentDescription(
            "Drawing thumbnail for ${imageData.timestamp}").assertHasClickAction()
        composeTestRule.onNodeWithContentDescription(
            "Drawing thumbnail for ${imageData.timestamp}").assertTextEquals(imageData.timestamp.toString())
        composeTestRule.onNodeWithContentDescription(
            "Share for ${imageData.timestamp}").assertIsDisplayed()
        composeTestRule.onNodeWithContentDescription(
            "Share for ${imageData.timestamp}").assertHasClickAction()

    }
//    @Test
//    fun testButtonNavigation() {
//        navController = mock(NavController::class.java)
//
//        val scenario = launchFragmentInContainer<DrawingSelectionFragment>()
//
//        scenario.onFragment { fragment ->
//            // Set the NavController for the fragment
//            Navigation.setViewNavController(fragment.requireView(), navController)
//
//            // Perform the click action on the Composable component
//            Espresso.onView(ViewMatchers.withContentDescription("New Drawing")).perform(ViewActions.click())
//
//            // Verify navigation
//            Mockito.verify(navController).navigate(R.id.drawingFragment)
//        }
//    }
}