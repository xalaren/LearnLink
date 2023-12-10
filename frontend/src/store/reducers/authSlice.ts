import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { ACCESS_KEY, EXPIRES_KEY, NICKNAME_KEY } from "../../helpers/constants";

interface IAuthState {
    isAuthenticated: boolean;
    accessToken: string;
    nickname: string;
}

interface IAuthPayload {
    accessToken: string;
    nickname: string;
}

function getInitialState(): IAuthState {
    const expiresIn = localStorage.getItem(EXPIRES_KEY) ?? null;

    if (expiresIn && new Date() > new Date(expiresIn)) {
        return {
            accessToken: '',
            nickname: '',
            isAuthenticated: false,
        }
    }

    const accessToken = localStorage.getItem(ACCESS_KEY) ?? '';
    const nickname = localStorage.getItem(NICKNAME_KEY) ?? '';

    return {
        accessToken: accessToken,
        nickname: nickname,
        isAuthenticated: Boolean(accessToken),
    }
}

const initialState: IAuthState = getInitialState();

export const authSlice = createSlice({
    name: "auth",
    initialState,
    reducers: {
        loginSuccess(state: IAuthState, action: PayloadAction<IAuthPayload>) {
            state.accessToken = action.payload.accessToken;
            state.isAuthenticated = Boolean(action.payload.accessToken);
            state.nickname = action.payload.nickname;

            const tokenExpires = new Date(new Date().getTime() + 24 * 60 * 60 * 1000);

            localStorage.setItem(ACCESS_KEY, action.payload.accessToken);
            localStorage.setItem(NICKNAME_KEY, action.payload.nickname);
            localStorage.setItem(EXPIRES_KEY, tokenExpires.toString())
        },
        // loginError(state: IAuthState, action: PayloadAction<IAuthErrorPayload>) {
        //     state.accessToken = '';
        //     state.isAuthenticated = false;
        //     state.nickname = '';
        //     state.error = action.payload.error;
        // },
        logout(state: IAuthState) {
            state.accessToken = '';
            state.isAuthenticated = false;
            state.nickname = '';

            localStorage.removeItem(ACCESS_KEY);
            localStorage.removeItem(NICKNAME_KEY);
            localStorage.removeItem(EXPIRES_KEY);
        }
    }
});

export default authSlice.reducer;