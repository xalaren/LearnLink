import { Loader } from "../components/Loader.tsx";
import { ErrorModal } from "../components/ErrorModal.tsx";
import { usePublicCourses } from "../hooks/CoursesHook.ts";
import { validate } from "../services/Validation.ts";
import { CoursesContainer } from "../components/CoursesContainer.tsx";

export function PublicPage() {
    const { courses, error, onError, loading } = usePublicCourses();
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



        return <CoursesContainer courses={courses} />;
    }

    return (
        <main className="main container">
            <div className="inner-container">
                <h2 className="main__title">Общедоступные курсы </h2>

                <ViewContent />

            </div>
        </main>
    )
}