import { useNavigate } from "react-router-dom";
import { useLogout } from "../hooks/LogoutHook";
import { useGlobalAuthorizationState } from "../hooks/GlobalStateHook";

export function HeaderNav() {
    const navigate = useNavigate();
    const logout = useLogout();
    const { isAuthorized } = useGlobalAuthorizationState();

    return (
        <>
            {
                isAuthorized &&
                <nav className="header__nav">
                    <a className="white-link">Мои курсы</a>
                    <a className="white-link">Личный кабинет</a>
                    <a className="white-link" onClick={logout}>Выйти</a>
                </nav>
            }

            {
                !isAuthorized &&
                <nav className="header__nav">
                    <button className="white-tp-button" onClick={() => navigate('/register')}>Регистрация</button>
                    <button className="white-tp-button" onClick={() => navigate('/login')}>Войти</button>
                </nav>
            }
        </>
    );
}