import { AxiosError } from "axios";
import { USER_ENDPOINTS_URL } from "../models/constants";
import { useState } from "react";
import { IAuthModel } from "../models/authModel";
import { IValueResponse, IVoidResponse } from "../models/response";
import { User } from "../models/user";
import axiosInstance from "../axios_config/axiosConfig";

export function useLogin() {
    const [error, setError] = useState('');
    const [authModel, setAuthModel] = useState<IAuthModel>();

    const loginQuery = async (nickname: string, password: string) => {
        try {
            const response = await axiosInstance.post<IValueResponse<string>>(`${USER_ENDPOINTS_URL}login?nickname=${nickname}&password=${password}`);

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
            const response = await axiosInstance.post<IVoidResponse>(`${USER_ENDPOINTS_URL}register?password=${password}`, user);

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
            const response = (await axiosInstance.get<IValueResponse<User>>(`${USER_ENDPOINTS_URL}get`, {
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

export function useUpdateUserData() {
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');
    const [token, setToken] = useState('');

    const updateUserDataQuery = async (user: User, accessToken: string) => {
        try {
            const response = (await axiosInstance.post<IValueResponse<string>>(`${USER_ENDPOINTS_URL}update-user`, user, {
                headers: {
                    Authorization: `Bearer ${accessToken}`
                }
            }));

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }


            setToken(response.data.value);
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

    return { updateUserDataQuery, token, error, success, resetValues }
}

export function useUpdatePassword() {
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const updatePasswordQuery = async (userId: number, accessToken: string, oldPassword: string, newPassword: string,) => {
        try {
            const response = (await axiosInstance.post<IVoidResponse>(`${USER_ENDPOINTS_URL}update-pass?userId=${userId}&oldPassword=${oldPassword}&newPassword=${newPassword}`, {}, {
                headers: {
                    Authorization: `Bearer ${accessToken}`
                }
            }));

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

    return { updatePasswordQuery, error, success, resetValues }
}

export function useRemoveUser() {
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const removeUserQuery = async (userId: number, accessToken: string) => {
        try {
            const response = (await axiosInstance.delete<IVoidResponse>(`${USER_ENDPOINTS_URL}remove?userId=${userId}`, {
                headers: {
                    Authorization: `Bearer ${accessToken}`
                }
            }));

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

    return { removeUserQuery, error, success, resetValues }
}