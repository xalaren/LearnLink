import {useEffect, useState} from "react";
import {Course} from "../models/Course.ts";
import {GetPublicCoursesAsync, GetUserCourses} from "../queries/CourseQueries.ts";
import {AxiosError} from "axios";
import {useAuthorization} from "./GlobalStateHook.ts";
import {getAccessToken} from "../services/AccessToken.ts";

export function usePublicCourses() {
    const [courses, setCourses] = useState<Course[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    const onError = () => {
        setError('');
    };


    useEffect(() => {
        fetchPublicCourses();
    }, []);

    async function fetchPublicCourses() {
        try {
            setError('');

            const courses = await GetPublicCoursesAsync();

            if(courses) setCourses(courses);

            setLoading(false);
        }
        catch (e: unknown) {
            setLoading(false);

            const error = e as AxiosError;
            setError(error.message);
        }
    }

    return {courses, error, onError, loading }
}