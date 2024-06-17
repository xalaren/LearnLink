import { Outlet } from "react-router-dom";
import { ModuleState } from "../../contexts/ModuleContext";

function CourseNestedLayout() {
    return (
        <ModuleState>
            <Outlet />
        </ModuleState>
    );
}

export default CourseNestedLayout;