import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { User } from "../../models/User";
import { getUserAsync } from "../../queries/UserQueries";

interface AuthorizationState {
    isAuthorized: boolean;
    user: User | null;
    accessToken: string;
    authorizationError: string;
}

const ACCESS_KEY = 'accessKey';

function refreshState(): AuthorizationState {
    const token = localStorage.getItem(ACCESS_KEY);
    let isAuthorized = Boolean(token);
    let user = null;

    if (!isAuthorized || !token) {
        return {
            accessToken: '',
            isAuthorized: false,
            user: null,
            authorizationError: 'Пользователь не авторизован'
        }
    }

    try {
        getUserAsync(token!).then(data => {
            if (data && data.value) user = data.value;
        })
    }
    catch (err) {
        isAuthorized = false;
        localStorage.removeItem(ACCESS_KEY);
    }

    return {
        accessToken: token,
        isAuthorized: isAuthorized,
        user: user,
        authorizationError: '',
    }
}

interface AuthorzationPayload {
    accessToken: string;
    user: User;
}

export const AuthorizationSlice = createSlice({
    name: 'authorization',
    initialState: refreshState(),
    reducers: {
        onLogin(state: AuthorizationState, action: PayloadAction<AuthorzationPayload>) {
            state.accessToken = action.payload.accessToken;
            state.isAuthorized = true;
            state.user = action.payload.user;

            localStorage.setItem(ACCESS_KEY, action.payload.accessToken);
        },
        refresh(state: AuthorizationState) {
            const refreshed = refreshState();

            state.accessToken = refreshed.accessToken;
            state.isAuthorized = refreshed.isAuthorized;
            state.user = refreshed.user;
        },
        logout(state) {
            state.accessToken = '';
            state.isAuthorized = false;
            state.user = null;

            localStorage.removeItem(ACCESS_KEY);
        }
    }
})

export default AuthorizationSlice.reducer;