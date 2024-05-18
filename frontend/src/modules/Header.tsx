import { HeaderButtons } from "../components/HeaderNav/HeaderNavButtons";
import { useAppSelector, useAppDispatch } from "../hooks/redux";
import { HeaderDropdown } from "../components/Dropdown/HeaderDropdown";
import { logout } from "../store/actions/authActionCreators";
import { ProfileEditActions, ViewTypes } from "../models/enums";
import { fetchUser } from "../store/actions/userActionCreators";
import { useHistoryNavigation } from "../hooks/historyNavigation";
import logo from "../assets/img/learnlinklogo.svg";
import DropdownItem from "../components/Dropdown/DropdownItem";
import { DropdownState } from "../contexts/DropdownContext";
import { paths } from "../models/paths";


export function Header() {
    const { toNext } = useHistoryNavigation();
    const { user } = useAppSelector(state => state.userReducer);
    const dispatch = useAppDispatch();

    return (
        <header className="header">
            <div className="header__container container">
                <section className="header__logo logo-pic" onClick={() => toNext(paths.public())}>
                    <img className="logo-pic__img" src={logo} alt="Learn Link" />
                    <h1 className="logo-pic__title">Learn Link</h1>
                </section>

                <nav className="header__nav">
                    {!user && <HeaderButtons links={[
                        {
                            path: paths.login,
                            title: 'Войти',
                        },
                        {
                            path: paths.register,
                            title: 'Регистрация',
                        }
                    ]} />}

                    {user &&
                        <DropdownState>
                            <HeaderDropdown title={user.nickname} avatarUrl={user.avatarUrl}>
                                <DropdownItem title="Профиль"
                                    className="icon icon-accent icon-user"
                                    onClick={() => toNext(paths.profile.edit(ProfileEditActions.main))}
                                />
                                <DropdownItem title="Мои курсы"
                                    className="icon icon-accent icon-star"
                                    onClick={() => toNext(paths.profile.courses(ViewTypes.created))}
                                />
                                <DropdownItem title="Выйти"
                                    className="icon icon-accent icon-power"
                                    onClick={() => {
                                        dispatch(logout());
                                        dispatch(fetchUser());
                                        toNext(paths.public());
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
