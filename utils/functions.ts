// Imports

import { Request, Response, NextFunction } from "express";
import { IAny } from "./types";



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
        if (typeof object[i] === 'object' && object[i] != null) {
            const nestedFlattenedObject: IAny = shallow(object[i], prefix + i + '.');
            result = {...result, ...nestedFlattenedObject};
        } else {
            result[prefix + i] = object[i];
        }
    });
    
    return result as T;
}