import { AxiosError } from "axios";
import { useState } from "react";
import { LOCALROLE_ENDPOINTS_URL } from "../models/constants";
import axiosInstance from "../axios_config/axiosConfig";
import { IValueResponse, IVoidResponse } from "../models/response";
import { LocalRole } from "../models/localRole";

export function useGetLocalRoleByUserAtCourse() {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    const getLocalRoleQuery = async (courseId: number, userId: number, accessToken: string) => {
        try {
            setLoading(true);
            const response = await axiosInstance.get<IValueResponse<LocalRole>>(
                `${LOCALROLE_ENDPOINTS_URL}get/byUserAtCourse?courseId=${courseId}&userId=${userId}`, {
                headers: {
                    Authorization: `Bearer ${accessToken}`
                }
            });

            setLoading(false);

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }

            return response.data.value;

        } catch (err: unknown) {
            setError((err as AxiosError).message);
        }
    }

    const resetValues = () => {
        setLoading(false);
        setError('');
    }

    return { getLocalRoleQuery, loading, error, resetValues };
}

export function useListAllLocalRoles() {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    const listLocalRolesQuery = async (accessToken: string) => {
        try {
            setLoading(true);
            const response = await axiosInstance.get<IValueResponse<LocalRole[]>>(
                `${LOCALROLE_ENDPOINTS_URL}get/all`, {
                headers: {
                    Authorization: `Bearer ${accessToken}`
                }
            });

            setLoading(false);

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }

            return response.data.value;

        } catch (err: unknown) {
            setError((err as AxiosError).message);
        }
    }

    const resetValues = () => {
        setLoading(false);
        setError('');
    }

    return { listLocalRolesQuery, loading, error, resetValues };
}

export function useReassignUserLocalRole() {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const reassignUserLocalRoleQuery = async (
        requesterUserId: number,
        targetUserId: number,
        courseId: number,
        localRoleId: number,
        accessToken: string) => {
        try {
            setLoading(true);
            const response = await axiosInstance.post<IVoidResponse>(
                `${LOCALROLE_ENDPOINTS_URL}reassign?requesterUserId=${requesterUserId}&targetUserId=${targetUserId}&courseId=${courseId}&localRoleId=${localRoleId}`, {}, {
                headers: {
                    Authorization: `Bearer ${accessToken}`
                }
            });

            setLoading(false);

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }

            setSuccess(response.data.message || '');

        } catch (err: unknown) {
            setError((err as AxiosError).message);
        }
    }

    const resetValues = () => {
        setLoading(false);
        setError('');
    }

    return { reassignUserLocalRoleQuery, loading, error, success, resetValues };
}