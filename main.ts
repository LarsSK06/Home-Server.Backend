// Imports

import express, { Express, Request, Response, NextFunction } from "express";
import session from "express-session";
import mongoose from "mongoose";
import dotenv from "dotenv";



// Config

dotenv.config();

const app: Express = express();

app.use(express.json());
app.use(express.urlencoded({ extended: true }));

app.use(session({
    secret: process.env.SessionSecret!,
    saveUninitialized: false,
    resave: false
}));



// Routing

app.use("/auth", require("./routes/auth"));



// Endpoints