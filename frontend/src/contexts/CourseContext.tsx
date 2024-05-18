import { createContext, useEffect, useState } from "react";
import { Course } from "../models/course";
import { useGetCourse } from "../hooks/courseHooks";
import { useParams } from "react-router-dom";
import { ErrorModal } from "../components/Modal/ErrorModal";
import PopupLoader from "../components/Loader/PopupLoader";
import { useAppSelector } from "../hooks/redux";
import { useHistoryNavigation } from "../hooks/historyNavigation";
import { paths } from "../models/paths";
import { MainContainer } from "../components/MainContainer";

interface ICourseContext {
    course: Course | null;
    setCourseContextData: (course: Course | null) => void;
    fetchCourse: () => Promise<void>;
}

export const CourseContext = createContext<ICourseContext>({
    course: null,
    setCourseContextData: () => { },
    fetchCourse: async () => { }
});

export const CourseState = ({ children }: { children: React.ReactNode }) => {
    const [course, setCourse] = useState<Course | null>(null);

    const { user } = useAppSelector(state => state.userReducer);

    const getCourseHook = useGetCourse();

    const param = useParams<'courseId'>();

    useEffect(() => {
        if (!course) fetchCourse();
    }, [user]);

    async function fetchCourse() {
        if (!user) {
            return;
        }

        const result = await getCourseHook.getCourseQuery(Number(param.courseId), user.id);

        if (result) {
            setCourse(result);
        }
    }

    function setCourseContextData(course: Course | null) {
        if (null) return;
        setCourse(course);
    }

    return (
        <CourseContext.Provider value={{ course, setCourseContextData, fetchCourse }}>
            {children}
        </CourseContext.Provider>
    )
}