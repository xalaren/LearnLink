import { authSlice } from "../reducers/authSlice";
import { AppDispatch } from "../store"

export const loginSave = (accessToken: string, nickname: string) => {
    return async (dispatch: AppDispatch) => {
        dispatch(authSlice.actions.loginSuccess({
            accessToken: accessToken,
            nickname: nickname,
        }))
    }
}

export const logout = () => {
    return async (dispatch: AppDispatch) => {
        dispatch(authSlice.actions.logout());
    };
}