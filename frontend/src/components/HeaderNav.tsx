import { NavButton } from "../ui/NavButton";

export function HeaderNav() {

    return (
        <nav className="header__nav">
            <NavButton link="/">Регистрация</NavButton>
            <NavButton link="/">Войти</NavButton>
        </nav>
    );
}