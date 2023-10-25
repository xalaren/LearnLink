import axios, { AxiosError } from "axios";
import { User } from "../models/User";
import { handleInternalError } from "./InternalErrorHandler";

const url = '/api/Users/'

export async function loginAsync(nickname: string, password: string): Promise<ValueResponse<string> | undefined> {
    try {
        const response = (await axios.post<ValueResponse<string>>(`${url}login?nickname=${nickname}&password=${password}`)).data;

        if (response.success === false) {
            throw new AxiosError(response.message);
        }

        return response;
    }
    catch (err: unknown) {
        handleInternalError(err);
    }
}

export async function registerAsync(user: User, password: string): Promise<Response | undefined> {
    try {
        const response = await axios.post<Response>(`${url}register?password=${password}`, user);
        const data = response.data;

        if (!data.success) {
            throw new AxiosError(data.message);
        }

        return data;
    }
    catch (err: unknown) {
        handleInternalError(err);
    }
}

export async function getUserAsync(accessToken: string): Promise<ValueResponse<User> | undefined> {
    try {
        return (await axios.get<ValueResponse<User>>(`${url}get`, {
            headers: {
                Authorization: `Bearer ${accessToken}`
            }
        })).data;
    }
    catch (err: unknown) {
        handleInternalError(err);
    }
}

export async function removeUserAsync(userId: number, accessToken: string): Promise<Response | undefined> {
    try {
        return (await axios.delete<Response>(`${url}remove?userId=${userId}`, {
            headers: {
                Authorization: `Bearer ${accessToken}`
            }
        })).data;
    }
    catch (err: unknown) {
        handleInternalError(err);
    }
}

export async function updateUserAsync(user: User, accessToken: string): Promise<ValueResponse<string> | undefined> {
    try {
        return (await axios.post<ValueResponse<string>>(`${url}update-user`, user,{
            headers: {
                Authorization: `Bearer ${accessToken}`
            }
        })).data;
    }
    catch (err: unknown) {
        handleInternalError(err);
    }
}

export async function updateUserPasswordAsync(userId: number, accessToken: string, oldPassword: string, newPassword: string): Promise<ValueResponse<string> | undefined> {
    try {
        return (await axios.post<ValueResponse<string>>(`${url}update-pass?userId=${userId}&oldPassword=${oldPassword}&newPassword=${newPassword}`, {},{
            headers: {
                Authorization: `Bearer ${accessToken}`
            }
        })).data;
    }
    catch (err: unknown) {
        handleInternalError(err);
    }
}