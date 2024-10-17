// Imports

import { Schema, Document, Types, model } from "mongoose";



// Types

export interface IUser{
    name: string;
    email: string;
    password: string;
    admin: boolean;
    delete?: Date;
}

export type MUser =
    Document<unknown, {}, IUser> &
    IUser &
    { _id: Types.ObjectId; } &
    { __v?: number; };

export interface ISession{
    token: string;
    user?: IUser;
    [ key: string | number ]: any;
}

export type MSession =
    Document<unknown, {}, ISession> &
    ISession &
    { _id: Types.ObjectId; } &
    { _v?: number; };



// Models

export const User = model<IUser>("user", new Schema({
    name: { type: String, required: true },
    email: { type: String, required: true },
    password: { type: String, required: true },
    admin: { type: Boolean, required: true },
    delete: { type: Date, required: false }
}));

export const Session = model<ISession>("session", new Schema({
    token: { type: String, required: true },
    user: { type: String, required: false }
}));