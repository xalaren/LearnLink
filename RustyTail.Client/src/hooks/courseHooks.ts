import { useState } from "react";
import { Course } from "../models/course";
import { AxiosError } from "axios";
import { ValueResponse, VoidResponse } from "../models/response";
import { COURSE_ENDPOINTS_URL } from "../models/constants";
import axiosInstance from "../axios_config/axiosConfig";
import { ValueDataPage } from "../models/dataPage";
import { Participant } from "../models/participant";

const defaultPageSize = 4;

export function usePublicCourses() {
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);

    const publicCoursesQuery = async (searchText: string, page: number, size: number = defaultPageSize) => {
        try {
            setLoading(true);

            const response = (await axiosInstance.get<ValueResponse<ValueDataPage<Course[]>>>(`${COURSE_ENDPOINTS_URL}find/public?title=${searchText}&page=${page}&size=${size}`));

            if (!response.data.success) {
                throw new AxiosError(response.data.message!);
            }

            setLoading(false);

            return response.data.value;
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

    return { publicCoursesQuery, loading, error, resetValues };
}

export function useUserCourses() {
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);

    const userCoursesQuery = async (userId: number, accessToken: string, searchText: string, findSubscription: boolean, findUnavailable: boolean, page: number = 1, size: number = defaultPageSize) => {
        try {
            setLoading(true);

            const response = (await axiosInstance.get<ValueResponse<ValueDataPage<Course[]>>>(`${COURSE_ENDPOINTS_URL}find/user-courses?userId=${userId}&title=${searchText}&findSubscription=${findSubscription}&findUnavailable=${findUnavailable}&page=${page}&size=${size}`, {
                headers: {
                    Authorization: `Bearer ${accessToken}`
                }
            }));

            if (!response.data.success) {
                throw new AxiosError(response.data.message!);
            }

            setLoading(false);
            return response.data.value;
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

    return { userCoursesQuery, loading, error, resetValues };
}

export function useGetCourse() {
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);

    const getCourseQuery = async (courseId: number, userId: number = 0) => {
        try {
            setLoading(true);

            const response = (await axiosInstance.get<ValueResponse<Course>>(`${COURSE_ENDPOINTS_URL}get/any?userId=${userId}&courseId=${courseId}`));
            setLoading(false);

            if (!response.data.success) {
                throw new AxiosError(response.data.message!);
            }

            return response.data.value;
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

    return { getCourseQuery, loading, error, resetValues };
}

export function useCreateCourse() {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const createCourseQuery = async (title: string, isPublic: boolean, userId: number, accessToken: string, description?: string,) => {
        try {
            setLoading(true);

            const course = new Course(0, title, isPublic, false, description);
            const response = (await axiosInstance.post<VoidResponse>(`${COURSE_ENDPOINTS_URL}create?userId=${userId}`, course, {
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

    return { createCourseQuery, loading, error, success, resetValues };
}

export function useGetSubscriberStatus() {
    const [error, setError] = useState('');

    const getStatusesQuery = async (userId: number, courseId: number, accessToken: string) => {
        try {
            const subscribedResponse = await axiosInstance.get<ValueResponse<boolean>>(`${COURSE_ENDPOINTS_URL}get/status/subscriber?userId=${userId}&courseId=${courseId}`, {
                headers: {
                    Authorization: 'Bearer ' + accessToken
                }
            });

            if (!subscribedResponse.data.success) {
                throw new AxiosError(subscribedResponse.data.message);
            }

            return subscribedResponse.data.value;
        }
        catch (err: unknown) {
            setError((err as AxiosError).message);
        }
    }

    const resetError = () => {
        setError('');
    }

    return { getStatusesQuery, error, resetError };
}

export function useGetCreatorStatus() {
    const [error, setError] = useState('');

    const getStatusesQuery = async (userId: number, courseId: number, accessToken: string) => {
        try {
            const response = await axiosInstance.get<ValueResponse<boolean>>(`${COURSE_ENDPOINTS_URL}get/status/creator?userId=${userId}&courseId=${courseId}`, {
                headers: {
                    Authorization: 'Bearer ' + accessToken
                }
            });

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }

            return response.data.value;
        }
        catch (err: unknown) {
            setError((err as AxiosError).message);
        }
    }

    const resetError = () => {
        setError('');
    }

    return { getStatusesQuery, error, resetError };
}

export function useUpdateCourse() {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const updateCourseQuery = async (course: Course, userId: number, accessToken: string) => {
        try {
            setLoading(true);
            const response = (await axiosInstance.post<VoidResponse>(`${COURSE_ENDPOINTS_URL}update?userId=${userId}`, course, {
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

    return { updateCourseQuery, loading, error, success, resetValues };
}

export function useHideCourse() {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const hideCourseQuery = async (courseId: number, userId: number, accessToken: string) => {
        try {
            setLoading(true);
            const response = (await axiosInstance.post<VoidResponse>(`${COURSE_ENDPOINTS_URL}setUnavailable?userId=${userId}&courseId=${courseId}`, {}, {
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

    return { hideCourseQuery, loading, error, success, resetValues };
}

export function useRemoveCourse() {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const removeCourseQuery = async (courseId: number, userId: number, accessToken: string) => {
        try {
            setLoading(true);

            const response = (await axiosInstance.delete<VoidResponse>(`${COURSE_ENDPOINTS_URL}remove?userId=${userId}&courseId=${courseId}`, {
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

    return { removeCourseQuery, loading, error, success, resetValues };
}

export function useFindCourseParticipants() {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    const findParticipantsQuery = async (
        userId: number,
        accessToken: string,
        courseId: number,
        page: number,
        searchText?: string,
        size: number = defaultPageSize,) => {
        try {
            setLoading(true);

            const response = (await axiosInstance
                .get<ValueResponse<ValueDataPage<Participant[]>>>(
                    `${COURSE_ENDPOINTS_URL}find/participants?userId=${userId}&courseId=${courseId}&searchText=${searchText}&page=${page}&size=${size}`, {
                    headers: {
                        Authorization: `Bearer ${accessToken}`
                    }
                }));

            setLoading(false);

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }

            return response.data.value;
        }
        catch (err: unknown) {
            setError((err as AxiosError).message);
        }
    }

    const resetValues = () => {
        setLoading(false);
        setError('');
    }

    return { findParticipantsQuery, loading, error, resetValues };
}