import { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { useAuthorization } from "../hooks/GlobalStateHook.ts";
import { ErrorModal } from "../components/ErrorModal.tsx";
import { validate } from "../services/Validation.ts";
import { useLogout } from "../hooks/LogoutHook.ts";
import { Loader } from "../components/Loader.tsx";
import { CoursesContainer } from "../components/CoursesContainer.tsx";
import { Course } from "../models/Course.ts";
import { GetUserCourses } from "../queries/CourseQueries.ts";
import { AxiosError } from "axios";
import { ViewType } from "../models/Constants.ts";

export function CoursesPage() {
    const [viewType, setViewType] = useState(ViewType.Created);
    const [courses, setCourses] = useState<Course[]>([]);
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);

    const location = useLocation();
    const navigate = useNavigate();
    const { isAuthorized, user, token } = useAuthorization();
    const logout = useLogout();

    useEffect(() => {
        console.log(user);
    }, [viewType]);

    async function fetchUserCourses() {
        try {
            setError('');

            const courses = await GetUserCourses(user.id, token);

            if (courses) {
                setLoading(false);
                setCourses(courses);
            }

            console.log(courses);
        }
        catch (e: unknown) {
            setLoading(false);

            const error = e as AxiosError;
            setError(error.message);
        }
    }

    function ViewContent() {
        if (!isAuthorized) {
            return <ErrorModal error="Пользователь не авторизован" active={!isAuthorized} onClose={logout}></ErrorModal>
        }

        if (error) {
            return <ErrorModal error={error} active={validate(error)} onClose={() => setError('')}></ErrorModal>
        }

        if (loading) {
            return <Loader />
        }

        return <CoursesContainer courses={courses} />
    }

    return (
        <main className="main container">
            <div className="inner-container">
                <nav className="courses-type">
                    <div className={`course-type ${viewType === ViewType.Created && 'course-type-selected'}`}>
                        Созданные курсы
                    </div>

                    <div
                        className={`course-type ${viewType === ViewType.Subscribed && 'course-type-selected'}`}
                    >
                        Подписки
                    </div>
                </nav>

                <ViewContent />

            </div>
        </main>
    );
}