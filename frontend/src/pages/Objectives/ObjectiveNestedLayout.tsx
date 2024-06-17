import { Outlet } from "react-router-dom";
import { ObjectiveState } from "../../contexts/ObjectiveContext";

function ObjectiveNestedLayout() {
    return (
        <ObjectiveState>
            <Outlet />
        </ObjectiveState>
    );
}

export default ObjectiveNestedLayout;