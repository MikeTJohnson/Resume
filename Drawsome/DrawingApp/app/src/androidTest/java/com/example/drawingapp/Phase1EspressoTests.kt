package com.example.drawingapp

//import android.view.View
//import androidx.test.espresso.Espresso.onView
//import androidx.test.espresso.UiController
//import androidx.test.espresso.ViewAction
//import androidx.test.espresso.action.ViewActions.click
//import androidx.test.espresso.assertion.ViewAssertions.matches
//import androidx.test.espresso.matcher.ViewMatchers.isDisplayed
//import androidx.test.espresso.matcher.ViewMatchers.isRoot
//import androidx.test.espresso.matcher.ViewMatchers.withId
//import androidx.test.espresso.matcher.ViewMatchers.withText
//import androidx.test.ext.junit.rules.ActivityScenarioRule
//import androidx.test.ext.junit.runners.AndroidJUnit4
//import androidx.test.filters.LargeTest
//import org.junit.Rule
//import org.junit.Test
//import org.junit.runner.RunWith
//
//
//@RunWith(AndroidJUnit4::class)
//@LargeTest
//class KevinEspressoTest {
//
//    @get:Rule
//    val activityRule = ActivityScenarioRule(MainActivity::class.java)
//
//    @Test
//    fun testDrawButton() {
//        //Check splash screen
//        onView(withText("DRAWSOME")).check(matches(isDisplayed()))
//        onView(isRoot()).perform(waitFor(2500))
//        //Check after timer that now on Click to Draw screen
//        onView(withId(R.id.button)).check(matches(isDisplayed()))
//        onView(withText("Click to Draw")).check(matches(isDisplayed()))
//        onView(withText("Click to Draw")).perform(click())
//        //Check that now on drawing canvas screen
//        onView(withId(R.id.penActionButton)).check(matches(isDisplayed()))
//        //open color palet and select default color(blue)
//        onView(withId(R.id.colorActionButton)).perform(click())
//        onView(withText("ok")).check(matches(isDisplayed()))
//        onView(withText("ok")).perform(click())
//    }
//}
//
//fun waitFor(delay: Long): ViewAction? {
//    return object : ViewAction {
//        override fun getConstraints(): org.hamcrest.Matcher<View>? {
//            return isRoot()
//        }
//
//        override fun getDescription(): String {
//            return "wait for " + delay + "milliseconds"
//        }
//
//        override fun perform(uiController: UiController, view: View?) {
//            uiController.loopMainThreadForAtLeast(delay)
//        }
//    }
//}