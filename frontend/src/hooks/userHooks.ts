import axios, { AxiosError } from "axios";
import { BASE_URL } from "../helpers/constants";
import { useEffect, useState } from "react";
import { IAuthModel } from "../models/authModel";

const userEndpointsParentUrl: string = BASE_URL + 'Users/';

export function useLogin() {
    const [error, setError] = useState('');
    const [authModel, setAuthModel] = useState<IAuthModel>();

    const loginQuery = async (nickname: string, password: string) => {
        try {
            const response = await axios.post<ValueResponse<string>>(`${userEndpointsParentUrl}login?nickname=${nickname}&password=${password}`);

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