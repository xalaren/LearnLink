import { useEffect } from "react";
import { MainContainer } from "../components/MainContainer";
import Profile from "../modules/Profile";
import { useAppSelector } from "../hooks/redux";
import { Paths } from "../helpers/enums";
import { useNavigate } from "react-router-dom";

function ProfilePage() {
    const { isAuthenticated } = useAppSelector(state => state.authReducer);
    const navigate = useNavigate();

    useEffect(() => {
        if (!isAuthenticated) {
            navigate(Paths.loginPath);
        }
    }, []);
    return (
        <MainContainer title="Профиль пользователя">
            <Profile />
        </MainContainer>
    );
}

export default ProfilePage;