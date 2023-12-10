import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { User } from "../../models/user";

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
        fetchUserError(state: IUserState, action: PayloadAction<Error>) {
            state.user = null;
            state.error = action.payload.message;
        }
    }
})


export default userSlice.reducer;