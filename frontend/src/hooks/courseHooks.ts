import { useState } from "react";
import { Course } from "../models/course";
import { AxiosError } from "axios";
import { IValueResponse, IVoidResponse } from "../models/response";
import { COURSE_ENDPOINTS_URL } from "../models/constants";
import axiosInstance from "../axios_config/axiosConfig";

export function usePublicCourses() {
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const [courses, setCourses] = useState<Course[]>();

    const publicCoursesQuery = async () => {
        try {
            setLoading(true);

            const response = (await axiosInstance.get<IValueResponse<Course[]>>(`${COURSE_ENDPOINTS_URL}get-public`));

            if (!response.data.success) {
                throw new AxiosError(response.data.message!);
            }

            setLoading(false);
            setCourses(response.data.value);
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

    return { publicCoursesQuery, courses, loading, error, resetValues };
}

export function useCreatedCourses() {
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const [courses, setCourses] = useState<Course[]>();

    const getCreatedCoursesQuery = async (userId: number, accessToken: string) => {
        try {
            setLoading(true);

            const response = (await axiosInstance.get<IValueResponse<Course[]>>(`${COURSE_ENDPOINTS_URL}get-user-courses?userId=${userId}`, {
                headers: {
                    Authorization: `Bearer ${accessToken}`
                }
            }));

            if (!response.data.success) {
                throw new AxiosError(response.data.message!);
            }

            setLoading(false);
            setCourses(response.data.value);
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

    return { getCreatedCoursesQuery, courses, loading, error, resetValues };
}

export function useSubscribedCourses() {
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const [courses, setCourses] = useState<Course[]>();

    const getSubscribedCoursesQuery = async (userId: number, accessToken: string) => {
        try {
            setLoading(true);

            const response = (await axiosInstance.get<IValueResponse<Course[]>>(`${COURSE_ENDPOINTS_URL}get-subscribed?userId=${userId}`, {
                headers: {
                    Authorization: `Bearer ${accessToken}`
                }
            }));

            if (!response.data.success) {
                throw new AxiosError(response.data.message!);
            }

            setLoading(false);
            setCourses(response.data.value);
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

    return { getSubscribedCoursesQuery, courses, loading, error, resetValues };
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

            const course = new Course(0, title, isPublic, description);
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