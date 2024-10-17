// Imports

import { IAny } from "./types";

import fs from "node:fs";
import path from "node:path";



// Types

type VerifyEnvType =
    | NumberConstructor
    | StringConstructor;

interface IVerifyEnvOptions{
    tplFile?: string;
    variables?: string[];
    types?: { [key: string]: VerifyEnvType }
}



// Functions

export function depth<T>(object: { [key: string]: any }, delimiter?: string): T{
    const result: IAny = {};

    Object.keys(object).forEach((i: string): void => {
        let cursor: any = result;
        const split: string[] = i.split(delimiter ?? ".");

        split
            .splice(0, split.length - 2)
            .forEach((x: string): void => {
                if(!(x in cursor))
                    cursor[x] = {};
                
                cursor = cursor[x];
            });

        cursor[split[split.length - 1]] = object[i];
    });

    return result as T;
}

export function shallow<T>(object: IAny, prefix: string = ''): T{
    let result: IAny = {};
    
    Object.keys(object).forEach((i: string): void => {
        if (typeof object[i] == 'object' && object[i] != null) {
            const nestedFlattenedObject: IAny = shallow(object[i], prefix + i + '.');
            result = {...result, ...nestedFlattenedObject};
        } else {
            result[prefix + i] = object[i];
        }
    });
    
    return result as T;
}

export function verifyEnv({ tplFile, variables, types }: IVerifyEnvOptions): void{
    ([] as string[]).concat(
        tplFile
            ? fs.readFileSync(path.join(__dirname, "..", "..", tplFile))
                .toString()
                .split("\n")
                .filter((i: string): boolean => i.split(" ").length > 1)
                .map((i: string): string => i.split(" ")[0])
                .concat(variables ?? [])
            : [],
        variables ?? []
    ).forEach((i: string): void => {
        if(!(i in process.env))
            throw new ReferenceError(
                `Missing environment variable "${i}"!`
            );
        
        if(types && i in types){
            try{ types[i](i); }
            catch{
                throw new TypeError(
                    `Mismatching types "${i}": ${types[i]}!`
                );
            }
        }
    });
}