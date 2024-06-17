import { Outlet } from "react-router-dom";
import { CourseState } from "../../contexts/CourseContext";

function CourseNestedLayout() {
    return (
        <CourseState>
            <Outlet />
        </CourseState>
    );
}

export default CourseNestedLayout;