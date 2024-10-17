// Imports

import bcrypt from "bcrypt";
import crypto from "node:crypto";

import { Router, Request, Response, NextFunction } from "express";
import { IUser, MSession, MUser, Session, User } from "../../utils/models";



// Config

const app: Router = Router();



// Types

interface IResponseBody{
    message: string;
    field?: string;
}

interface IPostUser{
    name: string;
    email: string;
    password: string;
}



// Endpoints

app.post("/sign-up", async (request: Request<undefined, undefined, IPostUser>, response: Response<IResponseBody>): Promise<void> => {
    const { name, email, password } = request.body;

    if(await User.exists({ email: email.toLowerCase() })){
        response
            .status(409)
            .send({
                message: "Email is already in use!",
                field: "email"
            });
        
        return;
    }

    const user: MUser = new User({
        name,
        email: email.toLowerCase(),
        password: await bcrypt.hash(password, 12),
        admin: false
    });

    try{ await user.save(); }
    catch(error: unknown){
        response
            .status(400)
            .send({
                message: `${error}`
            });
        
        return;
    }

    response
        .status(200)
        .send({
            message: "User created!"
        });
});

app.post("/log-in", async (request: Request<unknown, unknown, { email: string; password: string }>, response: Response<IResponseBody>): Promise<void> => {
    const { email, password } = request.body;
    
    const query: { email: string } =
        { email: email.toLowerCase() };

    if(!(await User.exists(query))){
        response
            .status(404)
            .send({
                message: "User with this email does not exist!",
                field: "email"
            });

        return;
    }

    const user: IUser = (await User.findOne(query).lean())!;

    if(!(await bcrypt.compare(password, user.password))){
        response
            .status(403)
            .send({
                message: "Invalid credentials!",
                field: "password"
            });

        return;
    }

    const session: MSession = new Session({
        token: crypto.randomBytes(10).toString("hex"),
        user
    });

    try{ await session.save(); }
    catch(error: unknown){
        response
            .status(400)
            .send({
                message: `${error}`
            });
        
        return;
    }

    response
        .header("authentication", session.token)
        .status(200)
        .send({
            message: "Successful login!"
        });
});



// Export

module.exports = app;

export default app;