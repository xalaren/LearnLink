import { HeaderNavButtons } from "../components/HeaderNavButtons";
import { useAppSelector, useAppDispatch } from "../hooks/redux";
import { HeaderDropdown } from "../components/HeaderDropdown";
import { logout } from "../store/actions/authActionCreators";
import { ViewTypes } from "../models/enums";
import { fetchUser } from "../store/actions/userActionCreators";
import { useHistoryNavigation } from "../hooks/historyNavigation";
import { Paths } from "../models/paths";
import logo from "../assets/img/learnlinklogo.svg";
import DropdownItem from "../ui/DropdownItem";
import { DropdownState } from "../contexts/DropdownContext";


export function Header() {
    const { toNext } = useHistoryNavigation();
    const { isAuthenticated, nickname } = useAppSelector(state => state.authReducer);
    const dispatch = useAppDispatch();

    return (
        <header className="header">
            <div className="header__container container">
                <section className="header__logo logo-pic" onClick={() => toNext(Paths.homePath)}>
                    <img className="logo-pic__img" src={logo} alt="Learn Link" />
                    <h1 className="logo-pic__title">Learn Link</h1>
                </section>



                {!isAuthenticated && <HeaderNavButtons links={[
                    {
                        path: Paths.loginPath,
                        title: 'Войти',
                    },
                    {
                        path: Paths.registerPath,
                        title: 'Регистрация',
                    }
                ]} />}

                {isAuthenticated &&
                    <DropdownState>
                        <HeaderDropdown title={nickname}>
                            <DropdownItem title="Профиль"
                                className="icon-user"
                                onClick={() => toNext(Paths.profilePath)}
                            />
                            <DropdownItem title="Мои курсы"
                                className="icon-star"
                                onClick={() => toNext(`${Paths.userCoursesPath}/${ViewTypes.created}`)}
                            />
                            <DropdownItem title="Выйти"
                                className="icon-power"
                                onClick={() => {
                                    dispatch(logout());
                                    dispatch(fetchUser());
                                    toNext(Paths.homePath);
                                }}
                            />
                        </HeaderDropdown>
                    </DropdownState>
                }
            </div>
        </header >
    )
}
