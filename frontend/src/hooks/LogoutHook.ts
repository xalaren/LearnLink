import { useNavigate } from "react-router-dom";
import { clearToken } from "../services/AccessToken"
import { useGlobalAuthorizationState } from "./GlobalStateHook";

export function useLogout() {
    const navigate = useNavigate();
    const { refresh } = useGlobalAuthorizationState();

    return function () {
        clearToken();
        navigate('/');
        refresh();
    }
}