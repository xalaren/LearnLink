import { useState } from "react";
import axiosInstance from "../axios_config/axiosConfig";
import { COMPLETION_ENDPOINTS_URL } from "../models/constants";
import { VoidResponse } from "../models/response"
import { AxiosError } from "axios";

export function useCompleteLesson() {
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');
    const [loading, setLoading] = useState(false);


    const completeLessonQuery = async (complete: boolean, userId: number, courseId: number, moduleId: number, lessonId: number, accessToken: string) => {
        try {
            setLoading(true);
            const response = await axiosInstance.post<VoidResponse>(`${COMPLETION_ENDPOINTS_URL}complete/lesson?userId=${userId}&courseId=${courseId}&moduleId=${moduleId}&lessonId=${lessonId}&complete=${complete}`, {}, {
                headers: {
                    Authorization: `Bearer ${accessToken}`
                }
            });

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }

            setLoading(false);
            setSuccess(response.data.message || '');
        }
        catch (err: unknown) {
            setLoading(false);
            setError((err as AxiosError).message);
        }
    }

    const resetValues = () => {
        setError('');
        setSuccess('');
        setLoading(false);
    }

    return { completeLessonQuery, error, success, loading, resetValues };
}