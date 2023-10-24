import { CourseItem } from "./CourseItem.tsx";
import { Loader } from "./Loader.tsx";
import { Course } from "../models/Course.ts";
import { GetPublicCoursesAsync } from "../queries/CourseQueries.ts";
import { useEffect, useState } from "react";
import { AxiosError } from "axios";
import { ErrorModal } from "./ErrorModal.tsx";

export function PublicPage() {
    const [courses, setCourses] = useState<Course[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [isModalActive, setIsModalActive] = useState(false);

    useEffect(() => {
        fetchCourses();
    }, []);

    async function fetchCourses() {
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
            openModal();
        }
    }

    const openModal = () => {
        setIsModalActive(true);
    };

    const closeModal = () => {
        setIsModalActive(false);
    };

    function ViewContent() {
        if (loading) return <Loader />

        if (error) {
            return (
                <>
                    <Loader />
                    <ErrorModal
                        error={error}
                        active={isModalActive}
                        onClose={closeModal} />
                </>
            );
        }

        if (courses.length == 0) return <p>Нет доступных курсов</p>;


        return courses.map(course => <CourseItem course={course} key={course.id} />);
    }

    return (
        <main className="main container">
            <div className="inner-container">
                <h2 className="main__title">Общедоступные курсы: </h2>

                <section className="course-container">
                    <ViewContent />
                </section>

            </div>
        </main>
    )
}