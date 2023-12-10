import profileImage from "../assets/img/profile.svg";
import { useAppSelector } from "../hooks/redux";
import { Paths } from "../helpers/enums";
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

function Profile() {
    const { isAuthenticated } = useAppSelector(state => state.authReducer);
    const { user } = useAppSelector(state => state.userReducer);
    const navigate = useNavigate();

    useEffect(() => {
        if (!isAuthenticated) {
            navigate(Paths.homePath);
        }
    }, [isAuthenticated, navigate]);


    return (
        <>
            {user && <div className="profile">
                <div className="profile__container">
                    <img className="profile__image" src={profileImage} alt="Профиль" />
                    <div className="profile__text">
                        <p className="medium-big">{user!.name} {user!.lastname}</p>
                        <p className="medium-little violet">@{user!.nickname} ({user!.role?.name})</p>
                    </div>
                </div>

                <nav className="profile__nav">
                    <button className="button-violet" onClick={() => { }}>Редактировать данные</button>
                    <button className="button-violet" onClick={() => { }}>Сменить пароль</button>
                    <button className="button-red" onClick={() => { }}>Удалить профиль</button>
                </nav>
            </div >
            }
        </>

    );
}

export default Profile;