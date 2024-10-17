// Types

export type Func<T = void> = () => T;
export type AFunc<T = void> = () => Promise<T>;



// Interfaces

export interface IResponseBody{
    message: string;
}

export interface IAny<T = any>{
    [key: string | number]: T;
}



// Enums

export enum Address{
    PhilipsHueBridge = "192.168.10.176"
}