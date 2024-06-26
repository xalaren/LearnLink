import { useState } from "react";
import axiosInstance from "../axios_config/axiosConfig";
import { ANSWER_ENDPOINTS_URL } from "../models/constants";
import { AxiosError } from "axios";
import { Answer } from "../models/answer";
import { ValueDataPage } from "../models/dataPage";
import { ValueResponse, VoidResponse } from "../models/response";

export function useAnswerQueries() {
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');
    const [loading, setLoading] = useState(false);

    const getAnswerQuery = async (requesterUserId: number, courseId: number, lessonId: number, answerId: number, accessToken: string) => {
        try {
            setLoading(true);
            const response = await axiosInstance.get<ValueResponse<Answer>>(`${ANSWER_ENDPOINTS_URL}get?requesterUserId=${requesterUserId}&courseId=${courseId}&lessonId=${lessonId}&answerId=${answerId}`, {
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

    const getAnswersFromObjectiveQuery = async (
        requesterUserId: number,
        courseId: number,
        lessonId: number,
        objectiveId: number,
        page: number,
        size: number,
        accessToken: string
    ) => {
        try {
            setLoading(true);
            const response = await axiosInstance.get<ValueResponse<ValueDataPage<Answer[]>>>(`${ANSWER_ENDPOINTS_URL}get/fromobjective?requesterUserId=${requesterUserId}&courseId=${courseId}&lessonId=${lessonId}&objectiveId=${objectiveId}&page=${page}&size=${size}`, {
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

    const createQuery = async (lessonId: number, answer: Answer, accessToken: string) => {
        try {
            setLoading(true);
            const response = await axiosInstance.post<VoidResponse>(`${ANSWER_ENDPOINTS_URL}create?lessonId=${lessonId}`, answer, {
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

    const updateQuery = async (lessonId: number, answer: Answer, removeFileContent: boolean, accessToken: string) => {
        try {
            setLoading(true);
            const response = await axiosInstance.post<VoidResponse>(`${ANSWER_ENDPOINTS_URL}update?lessonId=${lessonId}&removeFileContent=${removeFileContent}`, answer, {
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

    const removeQuery = async (lessonId: number, answerId: number, accessToken: string) => {
        try {
            setLoading(true);
            const response = await axiosInstance.delete<VoidResponse>(`${ANSWER_ENDPOINTS_URL}remove?lessonId=${lessonId}&answerId=${answerId}`, {
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

    return { getAnswerQuery, getAnswersFromObjectiveQuery, createQuery, updateQuery, removeQuery, error, success, loading, resetValues };
}