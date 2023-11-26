import { combineReducers, configureStore } from "@reduxjs/toolkit";
import AuthorizationReducer from './slices/AuthorizationSlice';

const rootReducer = combineReducers({
    AuthorizationReducer,
});

export function setupStore() {
    return configureStore({
        reducer: rootReducer
    });
}

export type RootState = ReturnType<typeof rootReducer>
export type AppStore = ReturnType<typeof setupStore>
export type AppDispatch = AppStore['dispatch']