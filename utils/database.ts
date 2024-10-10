// Import

import { IUseDatabaseOutput, useDatabase } from "./hooks/use-database";
import { IUser } from "./types";



// Databases

export const users: IUseDatabaseOutput<IUser> = useDatabase<IUser>({ name: "Users", readWriteOnAction: true });