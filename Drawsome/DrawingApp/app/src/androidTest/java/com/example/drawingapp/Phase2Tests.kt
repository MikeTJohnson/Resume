package com.example.drawingapp

import androidx.compose.ui.test.assertHasClickAction
import androidx.compose.ui.test.assertIsDisplayed
import androidx.compose.ui.test.junit4.createComposeRule
import androidx.compose.ui.test.onNodeWithContentDescription
import androidx.test.ext.junit.runners.AndroidJUnit4
import com.example.drawingapp.fragments.DrawingSelectionFragment
import org.junit.Rule
import org.junit.Test
import org.junit.runner.RunWith

@RunWith(AndroidJUnit4::class)
class Phase2Tests {

    @get:Rule
    val composeTestRule = createComposeRule()

    @Test
    fun testNewDrawingButton() {
        composeTestRule.setContent { DrawingSelectionFragment().NewDrawingButton() }
        composeTestRule.onNodeWithContentDescription("New Drawing").assertIsDisplayed()
        composeTestRule.onNodeWithContentDescription("New Drawing").assertHasClickAction()
    }

}