// Imports

import { useEmail, UseEmailFunction } from "./utils/hooks/use-email";
import { useState } from "./utils/hooks/use-state";

import express, { Express, Request, Response } from "express";
import session from "express-session";
import dotenv from "dotenv";
import cors from "cors";



// Config

dotenv.config();

const app: Express = express();

app.use(cors());
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

app.use(session({
    secret: process.env.SessionSecret!,
    saveUninitialized: false,
    resave: false
}));



// State hooks

const [error, setError] = useState<string>("");



// Email hooks

const sendErrorEmail: UseEmailFunction = useEmail({
    recipients: process.env.AdminEmail!,
    sender: "Kvihaugen no-reply<no-reply@kvihaugen.no>",
    subject: "API host failed!",
    content: `Could not host API due to error:\n${error}`,
    onError: (error: string): void =>
        console.log(`Error email error:\n${error}`)
});

const sendSuccessEmail: UseEmailFunction = useEmail({
    recipients: process.env.AdminEmail!,
    sender: "Kvihaugen no-reply<no-reply@kvihaugen.no>",
    subject: "API host success!",
    content: "API successfully hosting!",
    onError: (error: string): void =>
        console.log(`Success email error:\n${error}`)
});



// Routing

//app.use("/auth", require("./routes/auth"));
app.use("/light", require("./routes/light"));



// Endpoints

app.all("*", (_: Request, response: Response): void => {
    response
        .status(404)
        .send("Endpoint does not exist!");
});



// Hosting

(async (): Promise<void> => {
    app.listen(4000, "0.0.0.0", async (): Promise<void> => {
        console.log("Server online!");

        if(process.env.NODE_ENV == "production")
            await sendSuccessEmail();
    });
})();