import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { User } from "../../models/user";
import { IErrorString } from "../../models/interfaces";

interface IUserPayload {
    user: User;
}

interface IUserState {
    user?: User | null;
    error: string;
    loading: boolean;
}

const initialState: IUserState = {
    user: null,
    error: '',
    loading: false,
}

export const userSlice = createSlice({
    name: "user",
    initialState,
    reducers: {
        fetchUserSuccess(state: IUserState, action: PayloadAction<IUserPayload>) {
            state.user = action.payload.user;
            state.error = '';
            state.loading = false;
        },
        fetchUserError(state: IUserState, action: PayloadAction<IErrorString>) {
            state.user = null;
            state.error = action.payload.error;
            state.loading = false;
        },
        fetchingUser(state: IUserState) {
            state.loading = true;
        },
        reset(state: IUserState) {
            state.user = initialState.user;
            state.error = initialState.error;
            state.loading = initialState.loading;
        }
    }
})


export default userSlice.reducer;