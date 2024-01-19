package com.example.plugins

import org.jetbrains.exposed.dao.id.IntIdTable

object Drawing: IntIdTable() {
    val file_path = varchar("url", 255)
    val user_uid = varchar("user_uid", 28) // length of Firebase UID
    val user_name = varchar("user_name", 255) // user_name can be empty
    val timestamp = long("timestamp")

    init {
        check {
            file_path neq "" // require file_path to be non-empty
            user_uid neq "" // require user_uid to be non-empty
            // user_name can be empty
            timestamp neq 0L // require timestamp to be non-zero
        }
    }
}