// Imports

import { verifyEnv } from "./utils/functions";

import express, { Express, Request, Response } from "express";
import session from "express-session";
import mongoose from "mongoose";
import dotenv from "dotenv";
import cors from "cors";



// Config

dotenv.config();

verifyEnv({ tplFile: ".env.tpl" });

const app: Express = express();

app.use(cors());
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

app.use(session({
    secret: process.env.SessionSecret!,
    saveUninitialized: false,
    resave: false
}));



// Routing

app.use("/auth", require("./routes/auth"));
app.use("/light", require("./routes/light"));



// Endpoints

app.all("*", (_: Request, response: Response): void => {
    response
        .status(404)
        .send("Endpoint does not exist!");
});



// Hosting

(async (): Promise<void> => {
    try{ await mongoose.connect(process.env.DatabaseURI!); }
    catch{ return console.log("Could not connect to database!"); }

    app.listen(4000, "0.0.0.0", async (): Promise<void> => {
        console.log("Server online!");
    });
})();