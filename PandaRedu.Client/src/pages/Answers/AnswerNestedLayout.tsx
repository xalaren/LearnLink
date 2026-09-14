import { Outlet } from "react-router-dom";
import { AnswerState } from "../../contexts/AnswerContext";

function AnswerNestedLayout() {
    return (
        <AnswerState>
            <Outlet />
        </AnswerState>
    );
}

export default AnswerNestedLayout;