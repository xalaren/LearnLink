import { useState } from "react";
import { Course } from "../models/course";
import { AxiosError } from "axios";
import { IValueResponse, IVoidResponse } from "../models/response";
import { COURSE_ENDPOINTS_URL } from "../models/constants";
import axiosInstance from "../axios_config/axiosConfig";
import { ValueDataPage } from "../models/dataPage";

const defaultPageSize = 4;

export function usePublicCourses() {
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);

    const publicCoursesQuery = async (searchText: string, page: number, size: number = defaultPageSize) => {
        try {
            setLoading(true);

            const response = (await axiosInstance.get<IValueResponse<ValueDataPage<Course[]>>>(`${COURSE_ENDPOINTS_URL}find/public?title=${searchText}&page=${page}&size=${size}`));

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

            const response = (await axiosInstance.get<IValueResponse<ValueDataPage<Course[]>>>(`${COURSE_ENDPOINTS_URL}find/user-courses?userId=${userId}&title=${searchText}&findSubscription=${findSubscription}&findUnavailable=${findUnavailable}&page=${page}&size=${size}`, {
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
    const [course, setCourse] = useState<Course>();

    const getCourseQuery = async (courseId: number, userId: number = 0) => {
        try {
            setLoading(true);

            const response = (await axiosInstance.get<IValueResponse<Course>>(`${COURSE_ENDPOINTS_URL}get-any?userId=${userId}&courseId=${courseId}`));

            if (!response.data.success) {
                throw new AxiosError(response.data.message!);
            }

            setLoading(false);
            setCourse(response.data.value);
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

    return { getCourseQuery, course, loading, error, resetValues };
}

export function useCreateCourse() {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const createCourseQuery = async (title: string, isPublic: boolean, userId: number, accessToken: string, description?: string,) => {
        try {
            setLoading(true);

            const course = new Course(0, title, isPublic, false, description);
            const response = (await axiosInstance.post<IVoidResponse>(`${COURSE_ENDPOINTS_URL}create?userId=${userId}`, course, {
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

export function useUserCourseStatus() {
    const [error, setError] = useState('');
    const [isCreator, setCreator] = useState(false);
    const [isSubscriber, setSubscriber] = useState(false);

    const getStatusesQuery = async (userId: number, courseId: number, accessToken: string) => {
        try {
            const creatorResponse = await axiosInstance.get<IValueResponse<boolean>>(`${COURSE_ENDPOINTS_URL}get-creator-status?userId=${userId}&courseId=${courseId}`, {
                headers: {
                    Authorization: 'Bearer ' + accessToken
                }
            });

            if (!creatorResponse.data.success) {
                throw new AxiosError(creatorResponse.data.message);
            }

            const subscribedResponse = await axiosInstance.get<IValueResponse<boolean>>(`${COURSE_ENDPOINTS_URL}get-subscriber-status?userId=${userId}&courseId=${courseId}`, {
                headers: {
                    Authorization: 'Bearer ' + accessToken
                }
            });

            if (!subscribedResponse.data.success) {
                throw new AxiosError(creatorResponse.data.message);
            }

            setCreator(creatorResponse.data.value);
            setSubscriber(subscribedResponse.data.value);
        }
        catch (err: unknown) {
            setError((err as AxiosError).message);
        }
    }

    const resetError = () => {
        setError('');
    }

    return { getStatusesQuery, error, resetError, isCreator, isSubscriber };
}

export function useUpdateCourse() {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const updateCourseQuery = async (title: string, isPublic: boolean, userId: number, courseId: number, accessToken: string, description?: string,) => {
        try {
            setLoading(true);

            const course = new Course(courseId, title, isPublic, description);
            const response = (await axiosInstance.post<IVoidResponse>(`${COURSE_ENDPOINTS_URL}update?userId=${userId}`, course, {
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

export function useRemoveCourse() {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const removeCourseQuery = async (userId: number, courseId: number, accessToken: string) => {
        try {
            setLoading(true);

            const response = (await axiosInstance.delete<IVoidResponse>(`${COURSE_ENDPOINTS_URL}remove?userId=${userId}&courseId=${courseId}`, {
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