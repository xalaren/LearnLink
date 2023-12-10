import { userSlice } from "../reducers/userSlice"
import { AppDispatch } from "../store"
import { User } from "../../models/user"
import { ACCESS_KEY, BASE_URL } from "../../helpers/constants"
import axios, { AxiosError } from "axios"
import { IValueResponse } from "../../models/response"

export const fetchUser = () => {
    return async (dispatch: AppDispatch) => {
        const token = localStorage.getItem(ACCESS_KEY);

        if (!token) dispatch(userSlice.actions.reset());

        try {
            const response = (await axios.get<IValueResponse<User>>(`${BASE_URL}Users/get`, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            }));

            if (!response.data.success) {
                throw new AxiosError(response.data.message);
            }

            dispatch(userSlice.actions.fetchUserSuccess({
                user: response.data.value,
            }));
        }
        catch (err) {
            dispatch(userSlice.actions.fetchUserError({
                error: (err as AxiosError).message,
            }));
        }
    }
}

export const resetUserState = () => {
    return (dispatch: AppDispatch) => {
        dispatch(userSlice.actions.reset());
    }
}