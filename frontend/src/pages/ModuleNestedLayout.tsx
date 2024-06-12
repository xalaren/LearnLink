import { Outlet } from "react-router-dom";
import { ModuleState } from "../contexts/ModuleContext";
import { CourseState } from "../contexts/CourseContext";

function CourseNestedLayout() {
    return (
        <ModuleState>
            <Outlet />
        </ModuleState>
    );
}

export default CourseNestedLayout;