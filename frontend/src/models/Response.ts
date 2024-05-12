export interface VoidResponse {
    statusCode: number;
    success: boolean;
    message?: string;
    innerErrorMessages?: string[];
}

export interface IValueResponse<T> extends VoidResponse {
    value: T;
}