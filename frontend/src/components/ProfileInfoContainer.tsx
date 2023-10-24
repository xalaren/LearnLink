import {User} from "../models/User.ts";
import profile from "../assets/profile.svg"
interface IProfileInfoProps {
    user: User;
}

export function ProfileInfoContainer({user} : IProfileInfoProps) {

    return(
        <div className="profile">
            <div className="profile__container">
                <img className="profile__image" src={profile} alt="Профиль"/>
                <div className="profile__text">
                    <p className="medium-big">{user.name} {user.lastname}</p>
                    <p className="medium-little violet">@{user.nickname}</p>
                </div>
            </div>

            <nav className="profile__nav">
                <button className="button-violet">Редактировать данные</button>
                <button className="button-violet">Сменить пароль</button>
                <button className="button-red">Удалить аккаунт</button>
            </nav>
        </div>
    );
}