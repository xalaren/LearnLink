import { createContext, useEffect, useState } from "react";
import { Course } from "../models/course";
import { useGetCourse } from "../hooks/courseHooks";
import { useParams } from "react-router-dom";
import { ErrorModal } from "../components/Modal/ErrorModal";
import { useAppSelector } from "../hooks/redux";
import { MainContainer } from "../components/MainContainer";
import { Loader } from "../components/Loader/Loader";

interface ICourseContext {
    course: Course | null;
    fetchCourse: () => Promise<void>;
}

export const CourseContext = createContext<ICourseContext>({
    course: null,
    fetchCourse: async () => { },
});

export const CourseState = ({ children }: { children: React.ReactNode }) => {
    const [course, setCourse] = useState<Course | null>(null);
    const [courseId, setCourseId] = useState(0);

    const { user } = useAppSelector(state => state.userReducer);

    const { getCourseQuery, loading, error, resetValues } = useGetCourse();

    const param = useParams<'courseId'>();

    useEffect(() => {
        if (!user) return;
        if (!param || Number(param.courseId) === courseId) return;
        fetchCourse();
    }, [user, param]);

    async function fetchCourse() {
        if (!user) {
            return;
        }

        const result = await getCourseQuery(Number(param.courseId), user.id);

        if (result) {
            setCourse(result);
            setCourseId(Number(param.courseId));
        }
    }

    return (
        <CourseContext.Provider value={{ course, fetchCourse }}>
            {error &&
                <MainContainer>
                    <ErrorModal active={Boolean(error)} error={error} onClose={resetValues}>

                    </ErrorModal>
                </MainContainer>
            }
            {!error && loading &&
                <MainContainer>
                    <Loader />
                </MainContainer>
            }

            {!error && !loading &&
                <>
                    {children}
                </>
            }

        </CourseContext.Provider>
    )
}