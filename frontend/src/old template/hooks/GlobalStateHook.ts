import {useContext} from "react";
import {AuthorizationContext} from "../context/AuthorizationContext.tsx";

export const useAuthorization = () => useContext(AuthorizationContext);