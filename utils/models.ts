// Imports

import { model, Model, Schema, Document, Types } from "mongoose";



// Types

interface IUser{
    name: string;
    email: string;
    password: string;
    phone: string;
    joined: Date;
}

export type MUser = Document<unknown, {}, IUser> & IUser & { _id: Types.ObjectId; } & { __v?: number; };



// Models

export const User = model<IUser>(
    "user",
    new Schema<IUser, Model<IUser>>({
        name: { type: String, required: true },
        email: { type: String, required: true },
        password: { type: String, required: true },
        joined: { type: Date, required: true }
    })
);