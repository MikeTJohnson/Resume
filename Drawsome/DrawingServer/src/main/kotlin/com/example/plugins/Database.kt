package com.example.plugins

import io.ktor.server.application.*
import org.jetbrains.exposed.sql.Database
import org.jetbrains.exposed.sql.SchemaUtils
import org.jetbrains.exposed.sql.transactions.transaction

object DBSettings {
    private val db by lazy { Database.connect(
        url = "jdbc:h2:mem:test;MODE=MYSQL;DB_CLOSE_DELAY=-1",
        driver = "org.h2.Driver")
    }

    fun init() {
        transaction(db) {
            SchemaUtils.create(Drawing)
        }
    }
}