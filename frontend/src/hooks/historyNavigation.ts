import { useNavigate } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "./redux";
import { saveNavigation } from "../store/actions/navigationActionCreators";

export function useHistoryNavigation() {
    const navigate = useNavigate();
    const dispatch = useAppDispatch();
    const path = useAppSelector(state => state.navigationReducer.prevPath);

    const toNext = (toPath: string) => {
        dispatch(saveNavigation(toPath));
        navigate(toPath);
    }

    const toPrev = () => {
        navigate(path);
    }

    return { toNext, toPrev };
}