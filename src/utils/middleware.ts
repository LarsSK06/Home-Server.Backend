// Imports

import { Request, Response, NextFunction, RequestHandler } from "express";



// Types

interface IRequireAuthenticationOptions{
    admin?: boolean;
}



// Functions

export function requireAuthentication({ admin }: IRequireAuthenticationOptions): RequestHandler{
    return async (request: Request, response: Response, next: NextFunction): Promise<any> => {
        
    };
}