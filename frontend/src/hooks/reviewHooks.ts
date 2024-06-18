import { useState } from "react";
import axiosInstance from "../axios_config/axiosConfig";
import { ValueResponse, VoidResponse } from "../models/response";
import { ANSWER_ENDPOINTS_URL, REVIEW_ENDPOINTS_URL } from "../models/constants";
import { AxiosError } from "axios";
import { Review } from "../models/review";

export function useReviewQueries() {
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');
    const [loading, setLoading] = useState(false);

    const getReviewByAnswerQuery = async (answerId: number, accessToken: string) => {
        try {
            setLoading(true);
            const response = await axiosInstance.get<ValueResponse<Review | null>>(`${REVIEW_ENDPOINTS_URL}get?answerId=${answerId}`, {
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

    const createQuery = async (answerId: number, review: Review, accessToken: string) => {
        try {
            setLoading(true);
            const response = await axiosInstance.post<VoidResponse>(`${REVIEW_ENDPOINTS_URL}create?answerId=${answerId}`, review, {
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

    const updateQuery = async (review: Review, accessToken: string) => {
        try {
            setLoading(true);
            const response = await axiosInstance.post<VoidResponse>(`${REVIEW_ENDPOINTS_URL}update`, review, {
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

    const removeQuery = async (reviewId: number, accessToken: string) => {
        try {
            setLoading(true);
            const response = await axiosInstance.delete<VoidResponse>(`${ANSWER_ENDPOINTS_URL}remove?reviewId=${reviewId}`, {
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

    return { getReviewByAnswerQuery, createQuery, updateQuery, removeQuery, error, success, loading, resetValues };
}