import { navigationSlice } from "../reducers/navigationSlice"
import { AppDispatch } from "../store"

export const saveNavigation = (path: string) => {
    return async (dispatch: AppDispatch) => {
        dispatch(navigationSlice.actions.save({
            path: path,
        }))
    }
}