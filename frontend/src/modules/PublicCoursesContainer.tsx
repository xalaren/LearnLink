import { useEffect, useState } from "react";
import { CoursesContainer } from "../components/CoursesContainer";
import { usePublicCourses } from "../hooks/courseHooks";
import { Loader } from "../ui/Loader";
import { ErrorModal } from "../components/ErrorModal";

function PublicCoursesContainer() {
    const { publicCoursesQuery, courses, loading, error, resetValues } = usePublicCourses();
    const [isErrorModalActive, setErrorModalActive] = useState(false);

    useEffect(() => {
        fetchPublicCourses();
    }, []);

    useEffect(() => {

        if (error) {
            setErrorModalActive(true);
            return;
        }

    }, [error]);

    async function fetchPublicCourses() {
        await publicCoursesQuery();
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

export default PublicCoursesContainer;