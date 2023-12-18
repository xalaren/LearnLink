import { useState } from "react";
import { Module } from "../models/module";
import { AxiosError } from "axios";
import axiosInstance from "../axios_config/axiosConfig";
import { MODULE_ENDPOINRS_URL } from "../models/constants";
import { IValueResponse, IVoidResponse } from "../models/response";

export function useGetCourseModules() {
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const [modules, setModules] = useState<Module[]>();



    const getModulesQuery = async (courseId: number) => {
        try {
            setLoading(true);
            const response = await axiosInstance.get<IValueResponse<Module[]>>(`${MODULE_ENDPOINRS_URL}/get-course-modules?courseId=${courseId}`);

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }

            setLoading(false);
            setModules(response.data.value);
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

    return { getModulesQuery, modules, error, loading }
}

export function useCreateModules() {
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const [success, setSuccess] = useState(false);



    const createModulesQuery = async (courseId: number, title: string, accessToken: string, description?: string) => {
        try {
            setLoading(true);
            const module = new Module(title, description);
            const response = await axiosInstance.post<IVoidResponse>(`${MODULE_ENDPOINRS_URL}/create?courseId=${courseId}`, module, {
                headers: {
                    Authorization: `Bearer ${accessToken}`
                }
            });

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }

            setLoading(false);
            setSuccess(true);
        }
        catch (err: unknown) {
            setLoading(false);
            setError((err as AxiosError).message);
        }
    }

    const resetValues = () => {
        setError('');
        setLoading(false);
        setSuccess(false);
    }

    return { createModulesQuery, success, error, loading, resetValues }
}