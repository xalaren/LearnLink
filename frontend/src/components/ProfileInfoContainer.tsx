import {User} from "../models/User.ts";
import profile from "../assets/profile.svg"
import {useState} from "react";
import {Modal} from "./Modal.tsx";
import {ErrorModal} from "./ErrorModal.tsx";
import {validate} from "../services/Validation.ts";
import {useRemoveUser} from "../hooks/UsersManipulationHook.ts";
import {useNavigate} from "react-router-dom";
interface IProfileInfoProps {
    user: User;
}

export function ProfileInfoContainer({user} : IProfileInfoProps) {
    const[removeModalIsActive, setRemoveModalActive] = useState(false);
    const {removeUser, error, onError, success, onSuccess} = useRemoveUser(user.id!);
    const navigate = useNavigate();

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
                <button className="button-violet" onClick={() => navigate('edit')}>Редактировать данные</button>
                <button className="button-violet" onClick={() => navigate('edit/password')}>Сменить пароль</button>
                <button className="button-red" onClick={() => setRemoveModalActive(true)}>Удалить профиль</button>
            </nav>

            <Modal title="Удаление профиля" active={removeModalIsActive} onClose={() => setRemoveModalActive(false)}>
                    <p className="regular-red" style={{
                        marginBottom: "40px",
                    }}>Вы уверены, что хотите удалить профиль?</p>
                    <nav style={{
                        display: "flex",
                        justifyContent: "flex-end"
                    }}>
                        <button
                            style={{width: "80px", marginRight: "50px"}}
                            className="button-red"
                            onClick={removeUser}>
                                Да
                        </button>
                        <button
                            style={{width: "80px"}}
                            className="button-violet"
                            onClick={() => setRemoveModalActive(false)}>
                                Нет
                        </button>
                    </nav>
            </Modal>

            {error &&
                <ErrorModal active={validate(error)} onClose={onError} error={error}/>
            }

            {success &&
                <Modal title={"Успешно"} active={validate(success)} onClose={onSuccess}>
                    {success}
                </Modal>
            }
        </div>
    );
}