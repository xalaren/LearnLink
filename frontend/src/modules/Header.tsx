import { HeaderButtons } from "../components/HeaderNav/HeaderNavButtons";
import { useAppSelector, useAppDispatch } from "../hooks/redux";
import { HeaderDropdown } from "../components/Dropdown/HeaderDropdown";
import { logout } from "../store/actions/authActionCreators";
import { ViewTypes } from "../models/enums";
import { fetchUser } from "../store/actions/userActionCreators";
import { useHistoryNavigation } from "../hooks/historyNavigation";
import { Paths } from "../models/paths";
import logo from "../assets/img/learnlinklogo.svg";
import DropdownItem from "../components/Dropdown/DropdownItem";
import { DropdownState } from "../contexts/DropdownContext";


export function Header() {
    const { toNext } = useHistoryNavigation();
    const { user } = useAppSelector(state => state.userReducer);
    const dispatch = useAppDispatch();

    return (
        <header className="header">
            <div className="header__container container">
                <section className="header__logo logo-pic" onClick={() => toNext(Paths.homePath)}>
                    <img className="logo-pic__img" src={logo} alt="Learn Link" />
                    <h1 className="logo-pic__title">Learn Link</h1>
                </section>

                <nav className="header__nav">
                    {!user && <HeaderButtons links={[
                        {
                            path: Paths.loginPath,
                            title: 'Войти',
                        },
                        {
                            path: Paths.registerPath,
                            title: 'Регистрация',
                        }
                    ]} />}

                    {user &&
                        <DropdownState>
                            <HeaderDropdown title={user.nickname} avatarUrl={user.avatarUrl}>
                                <DropdownItem title="Профиль"
                                    className="icon icon-accent icon-user"
                                    onClick={() => toNext(Paths.editProfileMainPath)}
                                />
                                <DropdownItem title="Мои курсы"
                                    className="icon icon-accent icon-star"
                                    onClick={() => toNext(`${Paths.userCoursesPath}/${ViewTypes.created}/page/1`)}
                                />
                                <DropdownItem title="Выйти"
                                    className="icon icon-accent icon-power"
                                    onClick={() => {
                                        dispatch(logout());
                                        dispatch(fetchUser());
                                        toNext(Paths.homePath);
                                    }}
                                />
                            </HeaderDropdown>
                        </DropdownState>
                    }
                </nav>
            </div>
        </header >
    )
}
