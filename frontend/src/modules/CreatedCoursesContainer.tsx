/* eslint-disable react-hooks/exhaustive-deps */
import { useEffect, useState } from "react";
import { CoursesContainer } from "../components/CoursesContainer";
import { useCreatedCourses } from "../hooks/courseHooks";
import { Loader } from "../ui/Loader";
import { ErrorModal } from "../components/ErrorModal";
import { useAppSelector } from "../hooks/redux";

interface ICreatedCourseContainerProps {
    shouldUpdate: boolean;
    updateReset: () => void;
}

function CreatedCourseContainer({ shouldUpdate, updateReset }: ICreatedCourseContainerProps) {
    const { getCreatedCoursesQuery, courses, loading, error, resetValues } = useCreatedCourses();
    const [isErrorModalActive, setErrorModalActive] = useState(false);
    const { accessToken } = useAppSelector(state => state.authReducer);
    const { user } = useAppSelector(state => state.userReducer);

    useEffect(() => {
        fetchCourses();
    }, [accessToken, user]);

    useEffect(() => {
        if (shouldUpdate) {
            fetchCourses();
            updateReset();
        }
    }, [shouldUpdate])

    useEffect(() => {
        if (error) {
            setErrorModalActive(true);
            return;
        }

    }, [error]);

    async function fetchCourses() {
        if (accessToken && user) await getCreatedCoursesQuery(user.id!, accessToken);
    }

    function closeModal() {
        setErrorModalActive(false);
        resetValues();
    }

    return (
        <>
            {loading && <Loader />}
            {courses &&
                <section className="course-container">
                    <CoursesContainer courses={courses} />
                </section>
            }
            <ErrorModal active={isErrorModalActive} error={error} onClose={closeModal} />
        </>
    );
}

export default CreatedCourseContainer;