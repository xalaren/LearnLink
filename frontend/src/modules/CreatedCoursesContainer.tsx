/* eslint-disable react-hooks/exhaustive-deps */
import { useEffect, useState } from "react";
import { CoursesContainer } from "../components/CoursesContainer";
import { useCreatedCourses } from "../hooks/courseHooks";
import { Loader } from "../ui/Loader";
import { ErrorModal } from "../components/ErrorModal";
import { useAppSelector } from "../hooks/redux";

function CreatedCourseContainer() {
    const { getCreatedCoursesQuery, courses, loading, error, resetValues } = useCreatedCourses();
    const [isErrorModalActive, setErrorModalActive] = useState(false);
    const { accessToken } = useAppSelector(state => state.authReducer);
    const { user } = useAppSelector(state => state.userReducer);

    useEffect(() => {
        fetchCourses();
    }, [accessToken, user]);

    useEffect(() => {

        if (error) {
            setErrorModalActive(true);
            return;
        }

    }, [error, courses]);

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