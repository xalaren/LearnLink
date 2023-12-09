import { NavLink } from "../ui/NavLink";

export function HeaderAuthorizedNav() {
    return (
        <nav className="header__nav">
            <NavLink link="/">Мои курсы</NavLink>
            <NavLink link="/">Профиль</NavLink>
            <NavLink link="/">Выйти</NavLink>
        </nav>
    );
}