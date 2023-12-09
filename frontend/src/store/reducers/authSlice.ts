import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { ACCESS_KEY, NICKNAME_KEY } from "../../helpers/constants";

interface IAuthState {
    isAuthenticated: boolean;
    accessToken: string;
    nickname: string;
    error: string;
}

interface IAuthPayload {
    accessToken: string;
    nickname: string;
}

interface IAuthErrorPayload {
    error: string;
}

const initialState: IAuthState = {
    isAuthenticated: false,
    accessToken: '',
    nickname: '',
    error: '',
}


export const authSlice = createSlice({
    name: "auth",
    initialState,
    reducers: {
        loginSuccess(state: IAuthState, action: PayloadAction<IAuthPayload>) {
            state.accessToken = action.payload.accessToken;
            state.isAuthenticated = Boolean(action.payload.accessToken);
            state.nickname = action.payload.nickname;
            state.error = '';

            localStorage.setItem(ACCESS_KEY, action.payload.accessToken);
            localStorage.setItem(NICKNAME_KEY, action.payload.nickname);
        },
        loginError(state: IAuthState, action: PayloadAction<IAuthErrorPayload>) {
            state.accessToken = '';
            state.isAuthenticated = false;
            state.nickname = '';
            state.error = action.payload.error;
        },
        logout(state: IAuthState) {
            state.accessToken = '';
            state.isAuthenticated = false;
            state.nickname = '';
            state.error = '';

            localStorage.removeItem(ACCESS_KEY);
            localStorage.removeItem(NICKNAME_KEY);
        }
    }
});