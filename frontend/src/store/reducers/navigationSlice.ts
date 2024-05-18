import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { paths } from "../../models/paths";

interface INavigationState {
    nextPath: string;
    prevPath: string;
}

interface INavigationPayload {
    path: string;
}


const initialState: INavigationState = {
    nextPath: paths.home,
    prevPath: paths.home,
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