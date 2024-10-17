// Imports

import { createTransport, Transporter } from "nodemailer";
import { SentMessageInfo, Options } from "nodemailer/lib/smtp-transport";
import { AFunc } from "../types";



// Types

export interface IUseEmailInput{
    recipients: string | string[];
    sender?: string;
    subject: string;
    content: string;
    html?: boolean;
    onError?: (error: string) => void;
}

export type UseEmailFunction = AFunc;



// Functions

export function useEmail({ recipients, sender, subject, content, html, onError }: IUseEmailInput): UseEmailFunction{

    const transport: Transporter<SentMessageInfo, Options> = createTransport({
        host: process.env.EmailService,
        port: 465,
        secure: true,
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
                subject,
                text: html
                    ? undefined
                    : content,
                html: html
                    ? content
                    : undefined
            });
            console.log(`Sent email to ${recipients}!`);
        }
        catch(error){
            if(onError) onError(`${error}`);
            else console.log(`Email error:\n${error}`);
        }
    };
}