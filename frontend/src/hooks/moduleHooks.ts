import { useState } from "react";
import { Module } from "../models/module";
import { AxiosError } from "axios";
import axiosInstance from "../axios_config/axiosConfig";
import { MODULE_ENDPOINRS_URL } from "../models/constants";
import { IValueResponse } from "../models/response";

export function useGetCourseModules() {
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const [modules, setModules] = useState<Module[]>();



    const getCoursesQuery = async (courseId: number, accessToken: string) => {
        try {
            setLoading(true);
            const response = await axiosInstance.get<IValueResponse<Module[]>>(`${MODULE_ENDPOINRS_URL}/get-course-modules?courseId=${courseId}`, {
                headers: {
                    Authorization: `Bearer ${accessToken}`
                }
            });

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

    return { getCoursesQuery, modules, error, loading }
}