import { MainContainer } from "../components/MainContainer";
import Profile from "../modules/Profile";

function ProfilePage() {
    return (
        <MainContainer title="Профиль пользователя">
            <Profile />
        </MainContainer>
    );
}

export default ProfilePage;