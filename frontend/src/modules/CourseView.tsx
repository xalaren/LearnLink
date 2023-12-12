import { useEffect, useState } from "react";
import { useGetCourse } from "../hooks/courseHooks";
import { ErrorModal } from "../components/ErrorModal";
import { Loader } from "../ui/Loader";
import { useAppSelector } from "../hooks/redux";

interface ICourseViewProps {
    courseId: number;
}

function CourseView({ courseId }: ICourseViewProps) {
    const { getCourseQuery, course, loading, error: courseError, resetValues: resetCourseValues } = useGetCourse();

    const { isAuthenticated } = useAppSelector(state => state.authReducer);
    const user = useAppSelector(state => state.userReducer.user);
    const [localError, setLocalError] = useState('');

    useEffect(() => {
        fetchCourse();
    }, [courseId]);

    async function fetchCourse() {
        if (courseId !== 0) {
            await getCourseQuery(courseId, user?.id);
            if (courseError) setLocalError(courseError);
        }

    }

    function resetError() {
        resetCourseValues();
        setLocalError('');
    }

    return (
        <>
            {loading && <Loader />}

            {!localError && !loading && course &&
                <section className="course-view">
                    <div className="course-view__header">
                        <h2 className="course-view__title medium-big">{course.title}</h2>
                    </div>
                    <div className="course-view__description">
                        <p className="description regular">
                            {course.description}
                        </p>
                    </div>
                    <div className="course-view__modules">
                        <div className="modules__header">
                            <h3>Изучаемые модули:</h3>
                            <div className="modules__container">

                            </div>
                        </div>
                    </div>
                </section >
            }

            {localError && <ErrorModal active={Boolean(localError)} onClose={resetError} error={localError} />}
        </>
    );
}

export default CourseView;