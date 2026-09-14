import { useState } from "react";
import axiosInstance from "../axios_config/axiosConfig.ts";
import { ValueResponse, VoidResponse } from "../models/response";
import { LocalRole } from "../models/localRole.ts";
import { USER_COURSE_LOCAL_ROLE_ENDPOINTS_URL } from "../models/constants.ts";
import { AxiosError } from "axios";

export function useGetLocalRoleByUserAtCourse() {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    const getLocalRoleQuery = async (courseId: number, userId: number, accessToken: string) => {
        try {
            setLoading(true);
            const response = await axiosInstance.get<ValueResponse<LocalRole>>(
                `${USER_COURSE_LOCAL_ROLE_ENDPOINTS_URL}get?userId=${userId}&courseId=${courseId}`, {
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

export function useRequestReassignUserLocalRole() {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const requestReassignUserLocalRoleQuery = async (
        requesterUserId: number,
        targetUserId: number,
        courseId: number,
        localRoleId: number,
        accessToken: string) => {
        try {
            setLoading(true);
            const response = await axiosInstance.post<VoidResponse>(
                `${USER_COURSE_LOCAL_ROLE_ENDPOINTS_URL}request/reassign?requesterUserId=${requesterUserId}&targetUserId=${targetUserId}&courseId=${courseId}&localRoleId=${localRoleId}`, {}, {
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

    return { requestReassignUserLocalRoleQuery, loading, error, success, resetValues };
}