// Types

export type Func<T = void> = () => T;
export type AFunc<T = void> = () => Promise<T>;



// Interfaces

export interface IResponseBody{
    message: string;
}