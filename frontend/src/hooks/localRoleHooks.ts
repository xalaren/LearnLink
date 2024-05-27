import { useState } from "react";
import { LOCAL_ROLE_ENDPOINTS_URL } from "../models/constants";
import axiosInstance from "../axios_config/axiosConfig";
import { ValueResponse } from "../models/response";
import { LocalRole } from "../models/localRole";
import { AxiosError } from "axios";

export function useGetLocalRole() {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    const getLocalRoleQuery = async (roleId: number, accessToken: string) => {
        try {
            setLoading(true);
            const response = await axiosInstance.get<ValueResponse<LocalRole>>(
                `${LOCAL_ROLE_ENDPOINTS_URL}get/roleId=${roleId}`, {
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