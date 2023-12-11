import { useNavigate } from "react-router-dom";
import { HeaderNavButtons } from "../components/HeaderNavButtons";
import { useAppSelector, useAppDispatch } from "../hooks/redux";
import { DropdownButton } from "../components/DropdownButton";
import { logout } from "../store/actions/authActionCreators";
import userIcon from "../assets/img/user.svg";
import starIcon from "../assets/img/star.svg";
import powerIcon from "../assets/img/power.svg";
import { Paths } from "../models/enums";
import { fetchUser } from "../store/actions/userActionCreators";


export function Header() {
    const navigate = useNavigate();
    const { isAuthenticated, nickname } = useAppSelector(state => state.authReducer);
    const dispatch = useAppDispatch();

    return (
        <header className="header">
            <div className="container">
                <h1 className="header__title" onClick={() => navigate('/')}>Learn Link</h1>



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
                    <DropdownButton title={nickname}>
                        {[{
                            title: 'Профиль',
                            onClick: () => navigate(Paths.profilePath),
                            iconPath: userIcon,
                        },
                        {
                            //TODO: make real redirection to courses page
                            title: 'Мои курсы',
                            onClick: () => navigate(Paths.homePath),
                            iconPath: starIcon,
                        },
                        {
                            title: 'Выйти',
                            onClick: () => {
                                dispatch(logout())
                                    .then(() => {
                                        dispatch(fetchUser());
                                    })
                                    .then(() => {
                                        navigate(Paths.homePath);
                                    });
                            },
                            iconPath: powerIcon,
                        }]}
                    </DropdownButton>
                }
            </div>
        </header>
    )
}
