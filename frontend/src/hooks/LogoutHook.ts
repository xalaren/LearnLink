import { useNavigate } from "react-router-dom";
import { clearToken } from "../services/AccessToken"
import {useAuthorization} from "./GlobalStateHook";

export function useLogout() {
    const navigate = useNavigate();
    const { refreshAuthorization } = useAuthorization();

    return function () {
        clearToken();
        navigate('/');
        refreshAuthorization();
    }
}