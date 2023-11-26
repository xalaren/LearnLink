import { ProfileInfoContainer } from "../components/ProfileInfoContainer.tsx";
import { useAuthorization } from "../hooks/GlobalStateHook.ts";
import { useLogout } from "../hooks/LogoutHook.ts";
import { ErrorModal } from "../components/ErrorModal.tsx";

export function ProfilePage() {
    const { isAuthorized, user, token } = useAuthorization();
    const logout = useLogout();

    return (
        <main className="main container">
            <div className="inner-container">
                <h2 className="main__title">Профиль</h2>

                {!isAuthorized &&
                    <ErrorModal active={!isAuthorized} onClose={logout} error='Пользователь не авторизован' />
                }

                {user &&
                    <ProfileInfoContainer user={user} token={token} />
                }
            </div>
        </main>
    )
}