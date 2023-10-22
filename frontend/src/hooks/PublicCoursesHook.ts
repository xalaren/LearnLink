import {useEffect, useState} from "react";
import {Course} from "../models/Course.ts";
import {GetPublicCoursesAsync} from "../queries/CourseQueries.ts";

export function usePublicCourses() {
    const [courses, setCourses] = useState<Course[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    async function fetchCourses() {
        try {
            setError('');
            setLoading(true);


            const courses = await GetPublicCoursesAsync();
            setCourses(courses);

            setLoading(false);
        }
        catch (e: unknown) {
            const error = e as Error;
            setLoading(false);
            setError(error.message);
        }
    }

    useEffect(() => {
        fetchCourses();
    }, []);

    return { courses, error, loading }
}