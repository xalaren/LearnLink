import { Outlet } from "react-router-dom";
import { ModuleState } from "../contexts/ModuleContext";
import { CourseState } from "../contexts/CourseContext";

function CourseNestedLayout() {
    return (
        <CourseState>
            <ModuleState>
                <Outlet />
            </ModuleState>
        </CourseState>
    );
}

export default CourseNestedLayout;