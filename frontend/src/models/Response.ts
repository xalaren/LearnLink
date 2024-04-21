export interface IVoidResponse {
    statusCode: number;
    success: boolean;
    message?: string;
    innerErrorMessages?: string[];
}

export interface IValueResponse<T> extends IVoidResponse {
    value: T;
}