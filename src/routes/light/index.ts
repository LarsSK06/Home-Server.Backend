// Imports

import { Router, Request, Response } from "express";
import { Address } from "../../utils/types";



// Config

const app: Router = Router();



// Types

interface IGetLight{
    lights: {
        [key: string]: {
            state: {
                on: boolean;
                reachable: boolean;
            };
            type: string;
            name: string;
        };
    };
    groups: {
        [key: string]: {
            name: string;
            lights: string[];
            state: {
                all_on: boolean;
                any_on: boolean;
            };
        };
    };
}

interface ILight{
    id: string;
    state: {
        on: boolean;
        reachable: boolean;
    };
    type: string;
    name: string;
}

interface IGroup{
    id: string;
    name: string;
    lights: string[];
    state: {
        allOn: boolean;
        anyOn: boolean;
    }
}



// Endpoints

app.get("/", async (request: Request, response: Response): Promise<void> => {
    const bridgeResponse: globalThis.Response =
        await fetch(`http://${Address.PhilipsHueBridge}/api/${process.env.PhilipsHueUser}`);
    
    if(!bridgeResponse.ok){
        response
            .status(500)
            .send("Could not reach lights!");
        return;
    }

    const bridgeData: IGetLight =
        await bridgeResponse.json();
    
    response
        .status(200)
        .send(
            Object.keys(bridgeData.lights)
                .map((i: string): ILight => ({
                    ...bridgeData.lights[i],
                    id: i
                }))
        );
});



// Export

module.exports = app;

export default app;