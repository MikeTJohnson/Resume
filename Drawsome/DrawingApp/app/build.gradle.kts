val ktor_version: String by project

plugins {
    id("com.android.application")
    id("org.jetbrains.kotlin.android")
    id("kotlin-kapt")
    id("com.google.gms.google-services")
}

android {
    namespace = "com.example.drawingapp"
    compileSdk = 33

    defaultConfig {
        applicationId = "com.example.drawingapp"
        minSdk = 24
        targetSdk = 33
        versionCode = 1
        versionName = "1.0"

        testInstrumentationRunner = "androidx.test.runner.AndroidJUnitRunner"
        //Potentially use configurations.all if this doesn't solve the problem
        compileSdkPreview = "UpsideDownCake"
//        configurations.all {
//            resolutionStrategy {
//                force("androidx.emoji2:emoji2-views-helper:1.3.0")
//                force("androidx.emoji2:emoji2:1.3.0")
//            }
//        }
    }

    buildTypes {
        release {
            isMinifyEnabled = false
            proguardFiles(
                getDefaultProguardFile("proguard-android-optimize.txt"),
                "proguard-rules.pro"
            )
        }
    }
    compileOptions {
        sourceCompatibility = JavaVersion.VERSION_1_8
        targetCompatibility = JavaVersion.VERSION_1_8
    }
    kotlinOptions {
        jvmTarget = "1.8"
    }
    buildFeatures{
        dataBinding = true
        viewBinding = true
        compose = true
    }
    composeOptions {
        kotlinCompilerExtensionVersion = "1.5.2"
    }
    sourceSets {
        getByName("main") {
            assets {
                srcDirs("src/main/assets")
            }
        }
    }
}

dependencies {

    implementation("androidx.core:core-ktx:1.10.1")
    implementation("androidx.appcompat:appcompat:1.6.1")
    implementation("com.google.android.material:material:1.9.0")
    implementation("androidx.constraintlayout:constraintlayout:2.1.4")
    implementation("androidx.legacy:legacy-support-v4:1.0.0")
    implementation("androidx.recyclerview:recyclerview:1.3.0")
    implementation("androidx.test:core-ktx:1.5.0")
    implementation("androidx.lifecycle:lifecycle-runtime-testing:2.6.2")
    implementation("androidx.room:room-common:2.6.0-beta01")
    implementation("androidx.room:room-ktx:2.6.0-beta01")
    implementation("androidx.core:core-ktx:+")
    testImplementation("junit:junit:4.13.2")
    androidTestImplementation("androidx.test.ext:junit:1.1.5")
    androidTestImplementation("androidx.test:runner:1.5.2")
    androidTestImplementation("androidx.test:rules:1.5.0")
    androidTestImplementation("androidx.test.espresso:espresso-core:3.5.1")
    implementation("androidx.navigation:navigation-fragment-ktx:2.5.2")
    implementation("androidx.navigation:navigation-ui-ktx:2.5.2")
    implementation("androidx.test.espresso:espresso-idling-resource:3.3.0")
    //to get livedata + viewmodel stuff
    implementation("androidx.activity:activity-ktx:1.7.2")
    implementation("androidx.lifecycle:lifecycle-viewmodel-ktx:2.6.2")
    implementation("androidx.lifecycle:lifecycle-livedata-ktx:2.6.2")

    //Fragment stuff
    implementation("androidx.fragment:fragment-ktx:1.6.1")

    //color wheel
    implementation("com.github.QuadFlask:colorpicker:0.0.15")

    //Jetpack Compose
    val composeBom = platform("androidx.compose:compose-bom:2023.08.00")
    implementation(composeBom)
    androidTestImplementation(composeBom)
    // Material Design 3
    implementation("androidx.compose.material3:material3")
    // Android Studio Preview support
    implementation("androidx.compose.ui:ui-tooling-preview")
    debugImplementation("androidx.compose.ui:ui-tooling")
    // UI Tests
    androidTestImplementation("androidx.compose.ui:ui-test-junit4")
    debugImplementation("androidx.compose.ui:ui-test-manifest")
    // Optional - Integration with activities
    implementation("androidx.activity:activity-compose:1.7.2")
    // Optional - Integration with ViewModel
    implementation("androidx.lifecycle:lifecycle-viewmodel-compose:2.6.1")
    // Optional - Integration with LiveData
    implementation("androidx.compose.runtime:runtime-livedata")
    kapt("androidx.room:room-compiler:2.6.0-beta01")

    implementation("androidx.constraintlayout:constraintlayout-compose:1.1.0-alpha12")

    implementation("androidx.navigation:navigation-compose:2.7.2")

    //Jetpack Compose tests
    androidTestImplementation("androidx.compose.ui:ui-test-junit4:1.5.1")
    debugImplementation("androidx.compose.ui:ui-test-manifest:1.5.1")

    androidTestImplementation("androidx.fragment:fragment-testing:1.6.1")

    testImplementation("org.mockito:mockito-android:5.5.0")
    androidTestImplementation("org.mockito:mockito-android:5.5.0")
    androidTestImplementation("androidx.navigation:navigation-testing:2.7.3")
    androidTestImplementation("androidx.arch.core:core-testing:2.2.0")

    androidTestImplementation("androidx.compose.ui:ui-test-junit4:1.6.0-alpha07")
    androidTestImplementation("androidx.fragment:fragment-testing:1.7.0-alpha06")

    implementation(platform("com.google.firebase:firebase-bom:32.4.0"))
    implementation("com.firebaseui:firebase-ui-auth:8.0.2")


    implementation("com.android.volley:volley:1.2.1")

    implementation("io.ktor:ktor-client-core:2.3.5")
    implementation("io.ktor:ktor-client-cio:2.3.5")
    implementation("io.ktor:ktor-client-content-negotiation:$ktor_version")
    implementation("io.ktor:ktor-serialization-kotlinx-json:$ktor_version")

    implementation("com.google.code.gson:gson:2.10.1")

    implementation("io.coil-kt:coil-compose:2.4.0")
}
