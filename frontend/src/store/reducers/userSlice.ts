import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { User } from "../../models/user";
import { IErrorString } from "../../helpers/interfaces";

interface IUserPayload {
    user: User;
}

interface IUserState {
    user?: User | null;
    error: string;
}

const initialState: IUserState = {
    user: null,
    error: '',
};

export const userSlice = createSlice({
    name: "user",
    initialState,
    reducers: {
        fetchUserSuccess(state: IUserState, action: PayloadAction<IUserPayload>) {
            state.user = action.payload.user;
        },
        fetchUserError(state: IUserState, action: PayloadAction<IErrorString>) {
            state.user = null;
            state.error = action.payload.error;
        },
        reset(state: IUserState) {
            state.user = null;
            state.error = '';
        }
    }
})


export default userSlice.reducer;