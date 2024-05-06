import { AxiosError } from "axios";
import { useState } from "react";
import { LOCALROLE_ENDPOINTS_URL } from "../models/constants";
import axiosInstance from "../axios_config/axiosConfig";
import { IValueResponse } from "../models/response";
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