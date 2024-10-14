// Imports

import bcrypt from "bcrypt";

import { Router, Request, Response, NextFunction } from "express";
import { users } from "../../utils/database";
import { IUser } from "../../utils/types";



// Config

const app: Router = Router();



// Types

interface IResponseBody{
    message: string;
    field?: string;
}



// Endpoints

//app.post("/sign-up", async (request: Request<{}, IResponseBody | IUser, { name: string; email: string; password: string; }>, response: Response<IResponseBody | IUser>): Promise<void> => {
//    const { name, email, password } = request.body;

//    if(users.all().some((i: IUser): boolean => i.email.toLowerCase() == email.toLowerCase())){
//        response
//            .status(409)
//            .send({
//                message: "Email already in use!",
//                field: "email"
//            });
//        return;
//    }

//    const user: IUser = users.add({
//        id: Date.now(),
//        name,
//        email: email.toLowerCase(),
//        password: await bcrypt.hash(password, 12)
//    })[0];

//    response
//        .status(200)
//        .send(user);
//});

//app.post("/log-in", async (request: Request<{}, IResponseBody | IUser, { email: string; password: string; }, {}>, response: Response<IResponseBody | IUser>): Promise<void> => {
//    const { email, password } = request.body;

//    const user: IUser | undefined = users.all().find((i: IUser): boolean => i.email.toLowerCase() == email.toLowerCase());

//    if(!user){
//        response
//            .status(404)
//            .send({
//                message: "User does not exist!",
//                field: "email"
//            });
//        return;
//    }

//    if(!(await bcrypt.compare(password, user.password))){
//        response
//            .status(403)
//            .send({
//                message: "Invalid credentials!",
//                field: "password"
//            });
//        return;
//    }

//    response
//        .status(200)
//        .send({ ...user, password: "" } as IUser);
//});



// Export

module.exports = app;

export default app;