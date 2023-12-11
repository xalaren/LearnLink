import { useState } from "react";
import { Course } from "../models/course";
import axios, { AxiosError } from "axios";
import { IValueResponse } from "../models/response";
import { COURSE_ENDPOINTS_URL } from "../models/constants";



export function usePublicCourses() {
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const [courses, setCourses] = useState<Course[]>();

    const publicCoursesQuery = async () => {
        try {
            setLoading(true);

            const response = (await axios.get<IValueResponse<Course[]>>(`${COURSE_ENDPOINTS_URL}get-public`));

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