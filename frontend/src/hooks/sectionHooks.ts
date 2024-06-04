import { useState } from "react";
import axiosInstance from "../axios_config/axiosConfig";
import { ValueResponse, VoidResponse } from "../models/response";
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

export function useCreateSection() {
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const createSectionQuery = async (lessonId: number, section: Section, accessToken: string) => {
        try {
            const response = await axiosInstance.post<VoidResponse>(`${SECTIONS_ENDPOINTS_URL}create?lessonId=${lessonId}`, section, {
                headers: {
                    'Content-Type': 'multipart/form-data',
                    Authorization: `Bearer ${accessToken}`,
                }
            });

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }

            setSuccess(response.data.message!);
        }
        catch (err: unknown) {
            setError((err as AxiosError).message);
        }
    }

    const resetValues = () => {
        setError('');
        setSuccess('');
    }

    return { createSectionQuery, error, success, resetValues };
}

export function useUpdateSection() {
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const updateSectionQuery = async (lessonId: number, section: Section, accessToken: string) => {
        try {
            const response = await axiosInstance.post<VoidResponse>(`${SECTIONS_ENDPOINTS_URL}update?lessonId=${lessonId}`, section, {
                headers: {
                    'Content-Type': 'multipart/form-data',
                    Authorization: `Bearer ${accessToken}`,
                }
            });

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }

            setSuccess(response.data.message!);
        }
        catch (err: unknown) {
            setError((err as AxiosError).message);
        }
    }

    const resetValues = () => {
        setError('');
        setSuccess('');
    }

    return { updateSectionQuery, error, success, resetValues };
}

export function useChangeSectionOrder() {
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const changeSectionOrder = async (sectionId: number, lessonId: number, increase: boolean, accessToken: string) => {
        try {
            const response = await axiosInstance.post<VoidResponse>(`${SECTIONS_ENDPOINTS_URL}update/order?sectionId=${sectionId}&lessonId=${lessonId}&increase=${increase}`, {}, {
                headers: {
                    Authorization: `Bearer ${accessToken}`,
                }
            });

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }

            setSuccess(response.data.message!);
        }
        catch (err: unknown) {
            setError((err as AxiosError).message);
        }
    }

    const resetValues = () => {
        setError('');
        setSuccess('');
    }

    return { changeSectionOrder, error, success, resetValues };
}

export function useRemoveSection() {
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const sectionRemoveQuery = async (sectionId: number, accessToken: string) => {
        try {
            const response = await axiosInstance.delete<VoidResponse>(`${SECTIONS_ENDPOINTS_URL}remove?sectionId=${sectionId}`, {
                headers: {
                    Authorization: `Bearer ${accessToken}`,
                }
            });

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }

            setSuccess(response.data.message!);
        }
        catch (err: unknown) {
            setError((err as AxiosError).message);
        }
    }

    const resetValues = () => {
        setError('');
        setSuccess('');
    }

    return { sectionRemoveQuery, error, success, resetValues };
}