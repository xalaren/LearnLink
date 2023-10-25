import { CourseItem } from "./CourseItem.tsx";
import { Loader } from "./Loader.tsx";
import { ErrorModal } from "./ErrorModal.tsx";
import {usePublicCourses} from "../hooks/CoursesHook.ts";
import {validate} from "../services/Validation.ts";

export function PublicPage() {
    const {courses, error, onError, loading} = usePublicCourses();
    function ViewContent() {
        if (loading) return <Loader />

        if (error) {
            return (
                <>
                    <Loader />
                    <ErrorModal
                        error={error}
                        active={validate(error)}
                        onClose={onError} />
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