import { useState } from "react";
import axiosInstance from "../axios_config/axiosConfig";
import { ValueResponse, VoidResponse } from "../models/response";
import { Lesson } from "../models/lesson";
import { LESSON_ENDPOINTS_URL } from "../models/constants";
import { AxiosError } from "axios";

export function useGetModuleLessons() {
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);


    const getLessonsAtModuleQuery = async (userId: number, courseId: number, moduleId: number, accessToken: string) => {
        try {
            setLoading(true);
            const response = await axiosInstance.get<ValueResponse<Lesson[]>>(`${LESSON_ENDPOINTS_URL}get/atModule?userId=${userId}&courseId=${courseId}&moduleId=${moduleId}`, {
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

    return { getLessonsAtModuleQuery, error, loading, resetValues };
}

export function useCreateLesson() {
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const [success, setSuccess] = useState('');

    const createLessonQuery = async (userId: number, courseId: number, moduleId: number, title: string, accessToken: string, description?: string) => {
        try {
            setLoading(true);
            const lesson = new Lesson(title, description);
            const response = await axiosInstance.post<VoidResponse>(`${LESSON_ENDPOINTS_URL}create?userId=${userId}&courseId=${courseId}&moduleId=${moduleId}`, lesson, {
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
        setLoading(false);
        setSuccess('');
    }

    return { createLessonQuery, success, error, loading, resetValues }
}

export function useRemoveLesson() {
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const [success, setSuccess] = useState('');

    const removeLessonQuery = async (userId: number, courseId: number, moduleId: number, lessonId: number, accessToken: string) => {
        try {
            setLoading(true);
            const response = await axiosInstance.delete<VoidResponse>(`${LESSON_ENDPOINTS_URL}delete?userId=${userId}&courseId=${courseId}&moduleId=${moduleId}&lessonId=${lessonId}`, {
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
        setLoading(false);
        setSuccess('');
    }

    return { removeLessonQuery, success, error, loading, resetValues }
}