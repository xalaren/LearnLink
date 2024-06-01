import { Outlet } from "react-router-dom";
import { CourseState } from "../contexts/CourseContext";
import { LessonState } from "../contexts/LessonContext";

function CourseNestedLayout() {
    return (
        <CourseState>
            <LessonState>
                <Outlet />
            </LessonState>
        </CourseState>
    );
}

export default CourseNestedLayout;