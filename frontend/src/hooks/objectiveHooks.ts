import { useState } from "react";
import axiosInstance from "../axios_config/axiosConfig";
import { ValueResponse, VoidResponse } from "../models/response";
import { OBJECTIVE_ENDPOINTS_URL } from "../models/constants";
import { AxiosError } from "axios";
import { Objective } from "../models/objective";

export function useObjectiveQueries() {
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');
    const [loading, setLoading] = useState(false);

    const getObjectiveQuery = async (lessonId: number, obejctiveId: number, accessToken: string) => {
        try {
            setLoading(true);
            const response = await axiosInstance.get<ValueResponse<Objective>>(`${OBJECTIVE_ENDPOINTS_URL}get?lessonId=${lessonId}&objectiveId=${obejctiveId}`, {
                headers: {
                    Authorization: `Bearer ${accessToken}`
                }
            });


            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }

            setLoading(false);
            setSuccess(response.data.message || '');
            return (response.data.value);
        }
        catch (err: unknown) {
            setLoading(false);
            setError((err as AxiosError).message);
        }
    }

    const getObjectivesFromLessonQuery = async (lessonId: number, accessToken: string) => {
        try {
            setLoading(true);
            const response = await axiosInstance.get<ValueResponse<Objective[]>>(`${OBJECTIVE_ENDPOINTS_URL}get/fromlesson?lessonId=${lessonId}`, {
                headers: {
                    Authorization: `Bearer ${accessToken}`
                }
            });

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }

            setLoading(false);
            setSuccess(response.data.message || '');
            return (response.data.value);
        }
        catch (err: unknown) {
            setLoading(false);
            setError((err as AxiosError).message);
        }
    }

    const createQuery = async (lessonId: number, objective: Objective, accessToken: string) => {
        try {
            setLoading(true);
            const response = await axiosInstance.post<VoidResponse>(`${OBJECTIVE_ENDPOINTS_URL}create?lessonId=${lessonId}`, objective, {
                headers: {
                    'Content-Type': 'multipart/form-data',
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

    return { getObjectiveQuery, getObjectivesFromLessonQuery, createQuery, error, success, loading, resetValues };
}