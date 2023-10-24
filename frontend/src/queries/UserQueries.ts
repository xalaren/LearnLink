import axios, { AxiosError } from "axios";
import { User } from "../models/User";

const url = '/api/Users/'

export async function loginAsync(nickname: string, password: string): Promise<ValueResponse<string>> {
    const response = (await axios.post<ValueResponse<string>>(`${url}login?nickname=${nickname}&password=${password}`)).data;

    if (response.success === false) {
        throw new AxiosError(response.message);
    }

    return response;
}

export async function registerAsync(user: User, password: string): Promise<Response> {
    const response = await axios.post<Response>(`${url}register?password=${password}`, user);
    const data = response.data;

    if (!data.success) {
        throw new AxiosError(data.message);
    }

    return data;
}