export interface VoidResponse {
    statusCode: number;
    success: boolean;
    message?: string;
    innerErrorMessages?: string[];
}

export interface ValueResponse<T> extends VoidResponse {
    value: T;
}