import axios, { AxiosError } from "axios";
import { BASE_URL } from "../helpers/constants";
import { useState } from "react";
import { IAuthModel } from "../models/authModel";
import { IValueResponse, IVoidResponse } from "../models/response";
import { User } from "../models/user";

const userEndpointsParentUrl: string = BASE_URL + 'Users/';

export function useLogin() {
    const [error, setError] = useState('');
    const [authModel, setAuthModel] = useState<IAuthModel>();

    const loginQuery = async (nickname: string, password: string) => {
        try {
            const response = await axios.post<IValueResponse<string>>(`${userEndpointsParentUrl}login?nickname=${nickname}&password=${password}`);

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }

            setAuthModel({
                accessToken: response.data.value,
                nickname: nickname,
            });

            setError('');
        }
        catch (err: unknown) {
            setError((err as AxiosError).message);
        }
    }

    const resetError = () => {
        setError('');
    }

    return { loginQuery, error, resetError, authModel };
}

export function useRegister() {
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const registerQuery = async (nickname: string, password: string, lastname: string, name: string) => {
        try {
            const user = new User(nickname, lastname, name);
            const response = await axios.post<IVoidResponse>(`${userEndpointsParentUrl}register?password=${password}`, user);

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }

            setSuccess(response.data.message!);
        }
        catch (err: unknown) {
            setError((err as AxiosError).message);
        }
    }

    const resetValues = () => {
        setError('');
        setSuccess('');
    }

    return { registerQuery, error, success, resetValues };
}

export function useGetUser() {
    const [error, setError] = useState('');
    const [user, setUser] = useState<User>();

    const getUserQuery = async (accessToken: string) => {
        try {
            const response = (await axios.get<IValueResponse<User>>(`${userEndpointsParentUrl}get`, {
                headers: {
                    Authorization: `Bearer ${accessToken}`
                }
            }));

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }

            setUser(response.data.value);
        }
        catch (err: unknown) {
            setError((err as AxiosError).message);
        }
    }

    const resetError = () => {
        setError('');
    }

    return { getUserQuery, user, error, resetError }
}