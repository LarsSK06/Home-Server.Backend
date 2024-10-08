// Imports

import bcrypt from "bcrypt";

import { Router, Request, Response, NextFunction } from "express";
import { MUser, User } from "../../utils/models";



// Config

const app: Router = Router();



// Types

interface IResponseBody{
    message: string;
    field?: string;
}



// Endpoints

app.all("/log-in", async (request: Request<{}, IResponseBody, { email: string; password: string }, {}>, response: Response<IResponseBody>, next: NextFunction): Promise<void> => {
    const { email, password } = request.body;

    if(!(await User.exists({ email }))){
        response
            .status(404)
            .send({
                message: "User does not exist!",
                field: "email"
            });
        return;
    }

    const user: MUser = (await User.findOne({ email }))!;

    if(!(await bcrypt.compare(password, user.password))){
        response
            .status(403)
            .send({
                message: "Invalid credentials!",
                field: "password"
            });
        return;
    }
});



// Export

module.exports = app;

export default app;