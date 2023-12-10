import { combineReducers, configureStore } from "@reduxjs/toolkit";
import authReducer from "./reducers/authSlice";
import userReducer from "./reducers/userSlice";


const rootReducer = combineReducers({
    authReducer,
    userReducer,
});

const storeConfig = configureStore({
    reducer: rootReducer
})

export function setupStore() {
    return storeConfig;
}


export type RootState = ReturnType<typeof rootReducer>
export type AppStore = ReturnType<typeof setupStore>
export type AppDispatch = AppStore['dispatch']