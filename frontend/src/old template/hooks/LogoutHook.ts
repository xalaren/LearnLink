import { useNavigate } from "react-router-dom";
import {useAuthorization} from "./GlobalStateHook";

export function useLogout() {
    const navigate = useNavigate();
    const { refreshAuthorization } = useAuthorization();

    return function () {
        refreshAuthorization();
        navigate('/');
    }
}