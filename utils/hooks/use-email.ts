// Imports

import { createTransport, Transporter } from "nodemailer";
import { SentMessageInfo, Options } from "nodemailer/lib/smtp-transport";
import { AFunc } from "../types";



// Types

export interface IUseEmailOptions{
    recipients: string | string[];
    sender?: string;
    content: string;
    html?: boolean;
    onError: (error: string) => void;
}

export type UseEmailFunction = AFunc;



// Functions

export function useEmail({ recipients, sender, content, html, onError }: IUseEmailOptions): UseEmailFunction{

    const transport: Transporter<SentMessageInfo, Options> = createTransport({
        service: process.env.EmailService,
        port: Number(process.env.EmailServicePort),
        auth: {
            user: process.env.EmailUser,
            pass: process.env.EmailPass
        }
    });

    return async (): Promise<void> => {
        try{
            await transport.sendMail({
                to: recipients,
                from: sender,
                text: html
                    ? undefined
                    : content,
                html: html
                    ? content
                    : undefined
            })
        }
        catch(error){ onError(`${error}`); }
    };
}