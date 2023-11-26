import { useNavigate } from "react-router-dom";
import { useLogout } from "../hooks/LogoutHook";
import { useAppDispatch, useAppSelector } from "../hooks/redux.ts";

export function HeaderNav() {
    const navigate = useNavigate();
    const logout = useLogout();
    const dispatch = useAppDispatch();
    const { isAuthorized } = useAppSelector(state => state.AuthorizationReducer);


    return (
        <>
            {
                isAuthorized &&
                <nav className="header__nav">
                    <a className="white-link" onClick={() => navigate('/courses')}>Мои курсы</a>
                    <a className="white-link" onClick={() => navigate('/profile')}>Профиль</a>
                    <a className="white-link" onClick={() => {
                    }}>Выйти</a>
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