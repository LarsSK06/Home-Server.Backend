// Import

import fs from "node:fs";
import path from "node:path";

import { IAny } from "../types";



// Types

export interface IUseDatabaseInput<T>{
    name: string;
    readWriteOnAction?: boolean;
    initial?: T;
}

export interface IUseDatabaseOutput<T>{
    all: () => T[];
    add: (...value: T[]) => T[];
    remove: (...indexes: number[]) => T[];
}

type PrimitiveAcceptedValue =
    | string
    | number
    | boolean
    | null;

export type UseDatabaseAcceptedValue =
    | PrimitiveAcceptedValue
    | PrimitiveAcceptedValue[]
    | { [key: string]: PrimitiveAcceptedValue };



// Functions

export function useDatabase<T>({ name, readWriteOnAction, initial }: IUseDatabaseInput<T[]>): IUseDatabaseOutput<T>{
    const filePath: string = path.join(__dirname, "database", name);

    let data: T[] =
        JSON.parse(fs.readFileSync(filePath).toString())
        ?? initial
        ?? [];
    
    function read(): void{
        data = JSON.parse(fs.readFileSync(filePath).toString());
    }

    function write(): void{
        fs.writeFileSync(filePath, JSON.stringify(data, null, 4));
    }

    function all(): T[]{
        if(readWriteOnAction)
            read();

        return data;
    }

    function add(...value: T[]): T[]{
        data = data.concat(value);

        if(readWriteOnAction)
            write();

        return value;
    }

    function remove(...indexes: number[]): T[]{
        const deleting: T[] =
            data.filter((_: T, i: number): boolean => indexes.includes(i));

        data = data.filter((_: T, i: number): boolean => !indexes.includes(i));

        if(readWriteOnAction)
            write();

        return deleting;
    }

    return { all, add, remove };
}