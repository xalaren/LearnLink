import {useState} from "react";
import {useLogout} from "./LogoutHook.ts";
import {getAccessToken} from "../services/AccessToken.ts";
import {removeUserAsync} from "../queries/UserQueries.ts";
import {AxiosError} from "axios";
import {User} from "../models/User.ts";

export function useRemoveUser(user: User) {
    const[error, setError] = useState('');
    const[success, setSuccess] = useState('');
    const logout = useLogout();

    const onError = () => {
        setError('');
    }
    const onSuccess = () => {
        setSuccess('');
        logout()
    };
    const removeUser = async () => {
        const token = getAccessToken();

        try {
            if(!token) {
                return;
            }

            const response = (await removeUserAsync(user.id!, token))!;

            if(!response.success) {
                setError(response.message);
                return;
            }

            setSuccess(response.message);
        }
        catch(err: unknown) {
            setError((err as AxiosError).message);
        }
    }

    return { removeUser, error, onError, success, onSuccess }
}