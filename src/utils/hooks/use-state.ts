// Types

export type UseStateOutput<T> = [T, (value: T) => void];



// Functions

export function useState<T>(initial: T): UseStateOutput<T>{

    let state: T = initial;

    return [state, (value: T) => state = value];
}