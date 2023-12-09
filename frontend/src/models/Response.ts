export interface IVoidResponse {
    success: boolean;
    message?: string;
    innerErrorMessages?: string[];
}

export interface IValueResponse<T> extends IVoidResponse {
    value: T;
}