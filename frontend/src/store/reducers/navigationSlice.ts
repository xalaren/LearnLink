import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { Paths } from "../../models/enums";

interface INavigationState {
    nextPath: string;
    prevPath: string;
}

interface INavigationPayload {
    path: string;
}


const initialState: INavigationState = {
    nextPath: Paths.homePath,
    prevPath: Paths.homePath,
}

export const navigationSlice = createSlice({
    name: "navigation",
    initialState,
    reducers: {
        save(state: INavigationState, action: PayloadAction<INavigationPayload>) {
            state.prevPath = state.nextPath;
            state.nextPath = action.payload.path;
        }
    }
});

export default navigationSlice.reducer;