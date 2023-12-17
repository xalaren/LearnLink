import profileImage from "../assets/img/profile.svg";
import { useAppDispatch, useAppSelector } from "../hooks/redux";
import { Paths } from "../models/enums";
import { useEffect, useState } from "react";
import { Modal } from "../components/Modal";
import { useRemoveUser } from "../hooks/userHooks";
import { logout } from "../store/actions/authActionCreators";
import { resetUserState } from "../store/actions/userActionCreators";
import { ErrorModal } from "../components/ErrorModal";
import { SuccessModal } from "../components/SuccessModal";
import { useHistoryNavigation } from "../hooks/historyNavigation";

function Profile() {
    const { accessToken } = useAppSelector(state => state.authReducer);
    const { user } = useAppSelector(state => state.userReducer);
    const { removeUserQuery, error, success, resetValues } = useRemoveUser();
    const { toNext } = useHistoryNavigation();
    const dispatch = useAppDispatch();

    const [removeModalIsActive, setRemoveModalActive] = useState(false);
    const [isErrorModalActive, setErrorModalActive] = useState(false);
    const [isSuccessModalActive, setSuccessModalActive] = useState(false);

    useEffect(() => {
    }, [user]);

    useEffect(() => {
        if (error) {
            setErrorModalActive(true);
        }

        if (success) {
            setSuccessModalActive(true);
        }
    }, [dispatch, error, success])

    async function onClick() {
        if (user) await removeUserQuery(user.id!, accessToken);
        setRemoveModalActive(false);
    }

    function closeErrorModal() {
        setErrorModalActive(false);
        resetValues();
    }

    function closeSuccessModal() {
        setSuccessModalActive(false);
        dispatch(logout());
        dispatch(resetUserState());
        toNext(Paths.homePath);
    }


    return (
        <>
            {user &&
                <div className="profile">
                    <div className="profile__wrapper">
                        <div className="profile__container">
                            <img className="profile__image" src={profileImage} alt="Профиль" />
                            <div className="profile__text">
                                <p className="medium-big">{user!.name} {user!.lastname}</p>
                                <p className="medium-little-violet">@{user!.nickname} ({user!.role?.name})</p>
                            </div>
                        </div>

                        <nav className="profile__nav">
                            <button className="button-violet" onClick={() => { toNext(Paths.editUserPath) }}>Редактировать данные</button>
                            <button className="button-violet" onClick={() => { toNext(Paths.editPasswordPath) }}>Сменить пароль</button>
                            <button className="button-red" onClick={() => { setRemoveModalActive(true) }}>Удалить профиль</button>
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
                                    style={{ width: "80px", marginRight: "50px" }}
                                    className="button-red"
                                    onClick={onClick}>
                                    Да
                                </button>
                                <button
                                    style={{ width: "80px" }}
                                    className="button-violet"
                                    onClick={() => setRemoveModalActive(false)}>
                                    Нет
                                </button>
                            </nav>
                        </Modal>

                        <ErrorModal active={isErrorModalActive} error={error} onClose={closeErrorModal} />
                        <SuccessModal active={isSuccessModalActive} message={success} onClose={closeSuccessModal} />
                    </div>
                </div>
            }
        </>

    );
}

export default Profile;