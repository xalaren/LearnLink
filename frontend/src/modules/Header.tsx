import { useNavigate } from "react-router-dom";
import { HeaderNav, HeaderNavButtons } from "../components/HeaderNavButtons";
import { useAppSelector, useAppDispatch } from "../hooks/redux";
import { HeaderNavLinks } from "../components/HeaderNavLinks";
import { DropdownButton } from "../ui/DropdownButton";
import { logout } from "../store/actions/authActionCreators";


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
                        path: '/login',
                        title: 'Войти',
                    },
                    {
                        path: '/register',
                        title: 'Регистрация',
                    }
                ]} />}

                {isAuthenticated &&
                    <DropdownButton title={nickname}>
                        {[{
                            title: 'Профиль',
                            onClick: () => navigate('/profile'),
                        },
                        {
                            //TODO: make real redirection to courses page
                            title: 'Мои курсы',
                            onClick: () => navigate('/')
                        },
                        {
                            title: 'Выйти',
                            onClick: () => {
                                dispatch(logout());
                                navigate('/');
                            }
                        }]}
                    </DropdownButton>
                }
            </div>
        </header>
    )
}
