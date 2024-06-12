import { AxiosError } from "axios";
import { USER_ENDPOINTS_URL } from "../models/constants";
import { useState } from "react";
import { IAuthModel } from "../models/authModel";
import { ValueResponse, VoidResponse } from "../models/response";
import { User } from "../models/user";
import axiosInstance from "../axios_config/axiosConfig";
import { ValueDataPage } from "../models/dataPage";

export function useLogin() {
    const [error, setError] = useState('');

    const loginQuery = async (nickname: string, password: string): Promise<IAuthModel | undefined> => {
        try {
            const response = await axiosInstance.post<ValueResponse<string>>(`${USER_ENDPOINTS_URL}login?nickname=${nickname}&password=${password}`);

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }

            return ({
                accessToken: response.data.value,
                nickname: nickname,
            })
        }
        catch (err: unknown) {
            setError((err as AxiosError).message);
        }
    }

    const resetError = () => {
        setError('');
    }

    return { loginQuery, error, resetError };
}

export function useRegister() {
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const registerQuery = async (nickname: string, password: string, lastname: string, name: string, avatar?: File) => {
        try {
            const user = new User(nickname, lastname, name, avatar);

            const response = await axiosInstance.post<VoidResponse>(`${USER_ENDPOINTS_URL}register?password=${password}`, user, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                }
            });

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
            const response = (await axiosInstance.get<ValueResponse<User>>(`${USER_ENDPOINTS_URL}get`, {
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
            const response = (await axiosInstance.post<ValueResponse<string>>(`${USER_ENDPOINTS_URL}update/user`, user, {
                headers: {
                    'Content-Type': 'multipart/form-data',
                    Authorization: `Bearer ${accessToken}`
                }
            }));

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }


            setSuccess(response.data.message!);
            setToken(response.data.value);
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
            const response = (await axiosInstance.post<VoidResponse>(`${USER_ENDPOINTS_URL}update/pass?userId=${userId}&
oldPassword=${oldPassword}&newPassword=${newPassword}`, {}, {
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
            const response = (await axiosInstance.delete<VoidResponse>(`${USER_ENDPOINTS_URL}remove?userId=${userId}`, {
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

export function useFindUsers() {
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);

    const findUsersQuery = async (courseId: number, page: number, size: number, accessToken: string, searchText?: string) => {
        try {
            setLoading(true);
            const response = (await axiosInstance.get<ValueResponse<ValueDataPage<User[]>>>(`${USER_ENDPOINTS_URL}find/exceptCourseUsers?courseId=${courseId}&searchText=${searchText}&page=${page}&size=${size}&accessToken=${accessToken}`, {
                headers: {
                    Authorization: `Bearer ${accessToken}`
                }
            }));
            setLoading(false);

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }

            return response.data.value;
        }
        catch (err: unknown) {
            setLoading(false);
            setError((err as AxiosError).message);
        }
    }

    const resetValues = () => {
        setError('');
        setLoading(false);
    }

    return { findUsersQuery, error, loading, resetValues }
}