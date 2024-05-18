import { useState } from "react";
import { Module } from "../models/module";
import { AxiosError } from "axios";
import axiosInstance from "../axios_config/axiosConfig";
import { MODULE_ENDPOINTS_URL } from "../models/constants";
import { IValueResponse, VoidResponse } from "../models/response";

export function useGetCourseModules() {
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);

    const getModulesQuery = async (courseId: number, userId: number = 0) => {
        try {
            setLoading(true);
            const response = await axiosInstance.get<IValueResponse<Module[]>>(`${MODULE_ENDPOINTS_URL}get/atCourse?userId=${userId}&courseId=${courseId}`);

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

    return { getModulesQuery, error, loading, resetValues }
}

export function useCreateModules() {
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const [success, setSuccess] = useState('');

    const createModulesQuery = async (userId: number, courseId: number, title: string, accessToken: string, description?: string) => {
        try {
            setLoading(true);
            const module = new Module(title, description);
            const response = await axiosInstance.post<VoidResponse>(`${MODULE_ENDPOINTS_URL}create?userId=${userId}&courseId=${courseId}`, module, {
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

    return { createModulesQuery, success, error, loading, resetValues }
}

export function useGetModule() {
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const [module, setModule] = useState<Module>();

    const getModuleQuery = async (moduleId: number, accessToken: string) => {
        try {
            setLoading(true);

            if (!accessToken) {
                throw new AxiosError('Пользователь не авторизован');
            }

            const response = (await axiosInstance.get<IValueResponse<Module>>(`${MODULE_ENDPOINTS_URL}get?moduleId=${moduleId}`, {
                headers: {
                    Authorization: 'Bearer ' + accessToken
                }
            }));

            if (!response.data.success) {
                throw new AxiosError(response.data.message!);
            }

            setLoading(false);
            setModule(response.data.value);
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

    return { getModuleQuery, module, loading, error, resetValues };
}

export function useUpdateModule() {
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const [success, setSuccess] = useState('');

    const updateModuleQuery = async (moduleId: number, title: string, accessToken: string, description?: string) => {
        try {
            setLoading(true);
            const module = new Module(title, description, moduleId);
            const response = await axiosInstance.post<VoidResponse>(`${MODULE_ENDPOINTS_URL}/update`, module, {
                headers: {
                    Authorization: `Bearer ${accessToken}`
                }
            });

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }

            setLoading(false);
            setSuccess(response.data.message!);
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

    return { updateModuleQuery, success, error, loading, resetValues }
}

export function useRemoveModule() {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const removeModuleQuery = async (moduleId: number, accessToken: string) => {
        try {
            setLoading(true);

            const response = (await axiosInstance.delete<VoidResponse>(`${MODULE_ENDPOINTS_URL}remove?moduleId=${moduleId}`, {
                headers: {
                    Authorization: `Bearer ${accessToken}`
                }
            }));

            setLoading(false);

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
        setLoading(false);
        setError('');
        setSuccess('');
    }

    return { removeModuleQuery, loading, error, success, resetValues };
}