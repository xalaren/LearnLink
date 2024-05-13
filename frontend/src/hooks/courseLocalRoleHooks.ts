import { useState } from "react";
import axiosInstance from "../axios_config/axiosConfig";
import { COURSE_LOCAL_ROLE_ENDPOINTS_URL } from "../models/constants";
import { IValueResponse, VoidResponse } from "../models/response";
import { LocalRole } from "../models/localRole";
import { AxiosError } from "axios";

export function useListAllLocalRolesAtCourse() {
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

export function useRequestCreateCourseLocalRole() {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const requestCreateCourseLocalRoleQuery = async (requesterUserId: number, courseId: number, localRole: LocalRole, accessToken: string) => {
        try {
            setLoading(true);
            const response = await axiosInstance.post<VoidResponse>(
                `${COURSE_LOCAL_ROLE_ENDPOINTS_URL}request/create?requesterUserId=${requesterUserId}&courseId=${courseId}`, localRole, {
                headers: {
                    Authorization: `Bearer ${accessToken}`
                }
            });

            setLoading(false);

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }

            setSuccess(response.data.message || '');

        } catch (err: unknown) {
            setError((err as AxiosError).message);
        }
    }

    const resetValues = () => {
        setLoading(false);
        setError('');
        setSuccess('');
    }

    return { requestCreateCourseLocalRoleQuery, loading, error, success, resetValues };
}

export function useRequestUpdateCourseLocalRole() {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const requestUpdateCourseLocalRoleQuery = async (requesterUserId: number, courseId: number, localRole: LocalRole, accessToken: string) => {
        try {
            setLoading(true);
            const response = await axiosInstance.post<VoidResponse>(
                `${COURSE_LOCAL_ROLE_ENDPOINTS_URL}request/update?requesterUserId=${requesterUserId}&courseId=${courseId}`, localRole, {
                headers: {
                    Authorization: `Bearer ${accessToken}`
                }
            });

            setLoading(false);

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }

            setSuccess(response.data.message || '');

        } catch (err: unknown) {
            setError((err as AxiosError).message);
        }
    }

    const resetValues = () => {
        setLoading(false);
        setError('');
        setSuccess('');
    }

    return { requestUpdateCourseLocalRoleQuery, loading, error, success, resetValues };
}

export function useRequestDeleteCourseLocalRole() {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const requestDeleteCourseLocalRoleQuery = async (requesterUserId: number, courseId: number, localRoleId: number, accessToken: string) => {
        try {
            setLoading(true);
            const response = await axiosInstance.delete<VoidResponse>(
                `${COURSE_LOCAL_ROLE_ENDPOINTS_URL}request/delete?requesterUserId=${requesterUserId}&courseId=${courseId}&localRoleId=${localRoleId}`, {
                headers: {
                    Authorization: `Bearer ${accessToken}`
                }
            });

            setLoading(false);

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }

            setSuccess(response.data.message || '');

        } catch (err: unknown) {
            setError((err as AxiosError).message);
        }
    }

    const resetValues = () => {
        setLoading(false);
        setError('');
        setSuccess('');
    }

    return { requestDeleteCourseLocalRoleQuery, loading, error, success, resetValues };
}
