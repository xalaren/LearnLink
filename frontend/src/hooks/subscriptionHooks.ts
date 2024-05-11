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

export function useKick() {
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');
    const [loading, setLoading] = useState(false);

    const kickQuery = async (requesterUserId: number, targetUserId: number, courseId: number, accessToken: string) => {
        try {
            console.log(accessToken);

            setLoading(true);
            const response = await axiosInstance.delete<IVoidResponse>(`${SUBSCRIPTION_ENDPOINTS_URL}kick?requesterUserId=${requesterUserId}&targetUserId=${targetUserId}&courseId=${courseId}`, {
                headers: {
                    Authorization: `Bearer ${accessToken}`
                }
            });

            setLoading(false);

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }

            setSuccess(response.data.message || 'Успешно');
        }
        catch (err: unknown) {
            setLoading(false);
            setError((err as AxiosError).message);
        }
    }

    const resetValues = () => {
        setLoading(false);
        setError('');
        setSuccess('');
    }

    return { kickQuery, error, success, loading, resetValues };
}

export function useInvite() {
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const [success, setSuccess] = useState('');

    const inviteQuery = async (userId: number, accessToken: string, courseId: number, localRoleId: number, invitedUserId: number) => {
        try {
            const invitedUserIdentifiers: number[] = [invitedUserId];

            setLoading(true);
            const response = await axiosInstance.post<IVoidResponse>(`${SUBSCRIPTION_ENDPOINTS_URL}invite?userId=${userId}&courseId=${courseId}&localRoleId=${localRoleId}`, invitedUserIdentifiers, {
                headers: {
                    Authorization: `Bearer ${accessToken}`
                }
            });

            setLoading(false);

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }
            setSuccess(response.data.message || '');
        }
        catch (err: unknown) {
            setLoading(false);
            setError((err as AxiosError).message);
        }
    }

    const resetValues = () => {
        setLoading(false);
        setError('');
        setSuccess('');
    }

    return { inviteQuery, error, success, loading, resetValues };
}