import { useEffect, useState } from "react";
import { getAccessToken } from "../services/AccessToken";
import { checkAuthorize } from "../queries/AuthQueries";


export function useGlobalAuthorizationState() {
    const [isAuthorized, setAuthorized] = useState(false);

    useEffect(() => {
        refresh();
    });

    const refresh = () => {
        const accessToken = getAccessToken();

        if (!accessToken) {
            setAuthorized(false);
        }

        fetchCheckAuthorize(accessToken as string);
    };

    async function fetchCheckAuthorize(accessToken: string) {
        const response = await checkAuthorize(accessToken);

        setAuthorized(response.success);
    }

    return { isAuthorized, refresh };
}