interface Response {
    success: boolean;
    message: string;
    innerErrorMessages: string[];
}

interface ValueResponse<T> extends Response {
    value: T;
}