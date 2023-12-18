import { HeaderNavButtons } from "../components/HeaderNavButtons";
import { useAppSelector, useAppDispatch } from "../hooks/redux";
import { DropdownButton } from "../components/DropdownButton";
import { logout } from "../store/actions/authActionCreators";
import userIcon from "../assets/img/user.svg";
import starIcon from "../assets/img/star.svg";
import powerIcon from "../assets/img/power.svg";
import { ViewTypes } from "../models/enums";
import { fetchUser } from "../store/actions/userActionCreators";
import { useHistoryNavigation } from "../hooks/historyNavigation";
import { Paths } from "../models/paths";


export function Header() {
    const { toNext } = useHistoryNavigation();
    const { isAuthenticated, nickname } = useAppSelector(state => state.authReducer);
    const dispatch = useAppDispatch();

    return (
        <header className="header">
            <div className="container">
                <h1 className="header__title" onClick={() => toNext(Paths.homePath)}>Learn Link</h1>



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
                    <DropdownButton title={nickname} itemStyles={{ width: '150%' }}>
                        {[{
                            title: 'Профиль',
                            onClick: () => toNext(Paths.profilePath),
                            iconPath: userIcon,
                        },
                        {
                            title: 'Мои курсы',
                            onClick: () => toNext(`${Paths.userCoursesPath}/${ViewTypes.created}`),
                            iconPath: starIcon,
                        },
                        {
                            title: 'Выйти',
                            onClick: () => {
                                dispatch(logout());
                                dispatch(fetchUser());
                                toNext(Paths.homePath);
                            },
                            iconPath: powerIcon,
                        }]}
                    </DropdownButton>
                }
            </div>
        </header>
    )
}
