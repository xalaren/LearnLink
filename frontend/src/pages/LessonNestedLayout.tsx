import { Outlet } from "react-router-dom";
import { LessonState } from "../contexts/LessonContext";

function CourseNestedLayout() {
    return (
        <LessonState>
            <Outlet />
        </LessonState>
    );
}

export default CourseNestedLayout;