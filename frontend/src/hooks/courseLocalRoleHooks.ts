import { useState } from "react";
import axiosInstance from "../axios_config/axiosConfig";
import {COURSE_LOCAL_ROLE_ENDPOINTS_URL} from "../models/constants";
import { IValueResponse } from "../models/response";
import { LocalRole } from "../models/localRole";
import { AxiosError } from "axios";

export function useGetAtCourse() {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    const listLocalRolesQuery = async (courseId: number, accessToken: string) => {
        try {
            setLoading(true);
            const response = await axiosInstance.get<IValueResponse<LocalRole[]>>(
                `${COURSE_LOCAL_ROLE_ENDPOINTS_URL}get/atCourse?courseId=${courseId}`, {
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