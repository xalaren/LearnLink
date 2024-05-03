import { AxiosError } from "axios";
import axiosInstance from "../axios_config/axiosConfig";
import { SUBSCRIPTION_ENDPOINTS_URL } from "../models/constants";
import { IVoidResponse } from "../models/response";
import { useState } from "react";

export function useSubscription() {
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const [success, setSuccess] = useState(false);

    const subscribeQuery = async (userId: number, courseId: number, accessToken: string) => {
        try {
            console.log(accessToken);

            setLoading(true);
            const response = await axiosInstance.post<IVoidResponse>(`${SUBSCRIPTION_ENDPOINTS_URL}subscribe?userId=${userId}&courseId=${courseId}`, {}, {
                headers: {
                    Authorization: `Bearer ${accessToken}`
                }
            });

            setLoading(false);

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }
            setSuccess(true);
        }
        catch (err: unknown) {
            setLoading(false);
            setError((err as AxiosError).message);
        }
    }

    const unsubscribeQuery = async (userId: number, courseId: number, accessToken: string) => {
        try {
            setLoading(true);
            const response = await axiosInstance.delete<IVoidResponse>(`${SUBSCRIPTION_ENDPOINTS_URL}unsubscribe?userId=${userId}&courseId=${courseId}`, {
                headers: {
                    Authorization: `Bearer ${accessToken}`
                }
            });
            setLoading(false);

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }
            setSuccess(true);
        }
        catch (err: unknown) {
            setLoading(false);
            setError((err as AxiosError).message);
        }
    }

    const resetValues = () => {
        setLoading(false);
        setError('');
        setSuccess(false);
    }

    return { subscribeQuery, unsubscribeQuery, error, success, loading, resetValues };
}