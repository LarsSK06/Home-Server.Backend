// Imports

import { Request, Response, NextFunction, RequestHandler } from "express";
import { getUser } from "./functions";



// Functions

export function requireAuthentication({ admin }: { admin: boolean }): RequestHandler{
    return async (request: Request, response: Response, next: NextFunction): Promise<any> => {

        // get user
        // check user role

    };
}