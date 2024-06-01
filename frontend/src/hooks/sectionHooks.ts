import { useState } from "react";
import axiosInstance from "../axios_config/axiosConfig";
import { ValueResponse } from "../models/response";
import { Section } from "../models/section";
import { SECTIONS_ENDPOINTS_URL } from "../models/constants";
import { AxiosError } from "axios";

export function useGetLessonSections() {
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);

    const getSectionsFromLesson = async (lessonId: number, accessToken: string) => {
        try {
            setLoading(true);
            const response = await axiosInstance.get<ValueResponse<Section[]>>(`${SECTIONS_ENDPOINTS_URL}get/fromlesson?lessonId=${lessonId}`, {
                headers: {
                    Authorization: `Bearer ${accessToken}`
                }
            });

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }

            setLoading(false);
            return (response.data.value);
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

    return { getSectionsFromLesson, error, loading, resetValues };
}