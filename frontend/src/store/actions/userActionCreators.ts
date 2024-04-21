import { userSlice } from "../reducers/userSlice"
import { AppDispatch } from "../store"
import { User } from "../../models/user"
import { ACCESS_KEY, USER_ENDPOINTS_URL } from "../../models/constants"
import { AxiosError } from "axios"
import { IValueResponse } from "../../models/response"
import axiosInstance from "../../axios_config/axiosConfig"

export const fetchUser = () => {
    return async (dispatch: AppDispatch) => {
        dispatch(userSlice.actions.fetchingUser());

        const token = localStorage.getItem(ACCESS_KEY);

        if (!token) {
            dispatch(userSlice.actions.reset())
            return;
        }

        try {
            const response = await axiosInstance.get<IValueResponse<User>>(`${USER_ENDPOINTS_URL}get/auth`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },

            });


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