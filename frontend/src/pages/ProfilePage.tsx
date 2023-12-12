import { useEffect } from "react";
import { MainContainer } from "../components/MainContainer";
import Profile from "../modules/Profile";
import { useAppSelector } from "../hooks/redux";
import { Paths } from "../models/enums";
import { useHistoryNavigation } from "../hooks/historyNavigation";

function ProfilePage() {
    const { isAuthenticated } = useAppSelector(state => state.authReducer);
    const { toNext } = useHistoryNavigation();

    useEffect(() => {
        if (!isAuthenticated) {
            toNext(Paths.loginPath);
        }
    }, [isAuthenticated, toNext]);
    return (
        <MainContainer title="Профиль пользователя">
            <Profile />
        </MainContainer>
    );
}

export default ProfilePage;