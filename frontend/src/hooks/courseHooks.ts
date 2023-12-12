import { useState } from "react";
import { Course } from "../models/course";
import { AxiosError } from "axios";
import { IValueResponse } from "../models/response";
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

export function useGetCourse() {
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const [course, setCourse] = useState<Course>();

    const getCourseQuery = async (courseId: number, accessToken: string) => {
        try {
            setLoading(true);

            const response = (await axiosInstance.get<IValueResponse<Course>>(`${COURSE_ENDPOINTS_URL}get?courseId=${courseId}`, {
                headers: {
                    Authorization: `Bearer ${accessToken}`
                }
            }));

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