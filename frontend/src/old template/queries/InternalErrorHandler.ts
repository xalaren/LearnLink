import { AxiosError } from "axios";

export function handleInternalError(error: unknown): void {
    const axiosError = error as AxiosError;

    if (axiosError.response?.status === 500) {
        throw new AxiosError('Сервер недоступен');
    }

    throw axiosError;
}