import {useState} from "react";
import {useLogout} from "./LogoutHook.ts";
import {getAccessToken} from "../services/AccessToken.ts";
import {removeUserAsync} from "../queries/UserQueries.ts";
import {AxiosError} from "axios";

export function useRemoveUser(userId: number) {
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

            const response = (await removeUserAsync(userId, token))!;

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

// export function useUpdateUser(user: User) {
//     const [error, setError] = useState('');
//     const [success, setSuccess] = useState('');
//
//     const onError = () => {
//         setError('');
//     }
//     const onSuccess = () => {
//         setSuccess('');
//     };
//
//     async function fetchUpdateUserResult(user: User) {
//         try {
//             const token = getAccessToken();
//
//             if(!token) {
//                 return;
//             }
//             const response = (await updateUserAsync(user, token))!;
//
//             setSuccess(response.message);
//         }
//         catch (error: unknown) {
//             const axiosError = error as AxiosError;
//             setError(axiosError.message);
//         }
//     }
//
//     return {fetchUpdateUserResult, error, onError, success, onSuccess};
// }