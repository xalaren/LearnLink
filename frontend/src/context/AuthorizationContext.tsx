import React, {createContext, useEffect,  useState} from 'react';
import {User} from "../models/User.ts";
import {getUserAsync} from "../queries/UserQueries.ts";
import {getAccessToken} from "../services/AccessToken.ts";

interface IAuthorizationContext {
    isAuthorized: boolean;
    user: User | null;
    refreshAuthorization: () => void;
}

export const AuthorizationContext = createContext<IAuthorizationContext>({
    isAuthorized: true,
    user: null,
    refreshAuthorization: () => {}
})

export const AuthorizationState = ({ children }: {children: React.ReactNode}) => {
    const [isAuthorized, setAuthorized] = useState(true);
    const [user, setUser] = useState<User | null>(null);
    const refreshAuthorization = () => {
        fetchData();
    }

    useEffect(() => {
        refreshAuthorization();
        setInterval(() => refreshAuthorization(), 20 * 60 * 1000);
    }, [isAuthorized])
    async function fetchData() {
        try {
            const accessToken = getAccessToken();

            if(!accessToken) {
                setAuthorized(false);
                return;
            }

            const response = await getUserAsync(accessToken);

            if(!response)  {
                setAuthorized(false);
                return;
            }

            setAuthorized(response.success);
            setUser(response.value);
        } catch (error: unknown) {
            setAuthorized(false);
        }
    }

    return (
        <AuthorizationContext.Provider value={{isAuthorized, refreshAuthorization, user}}>
            { children }
        </AuthorizationContext.Provider>
    )
}