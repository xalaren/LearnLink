import { useEffect, useState } from "react";
import { Modal } from "../components/Modal/Modal";
import Checkbox from "../components/Checkbox";
import { useAppDispatch, useAppSelector } from "../hooks/redux";
import { NotificationType } from "../models/enums";
import { useRemoveUser } from "../hooks/userHooks";
import { useHistoryNavigation } from "../hooks/historyNavigation";
import { Paths } from "../models/paths";
import PopupNotification from "../components/PopupNotification";
import { logout } from "../store/actions/authActionCreators";
import { resetUserState } from "../store/actions/userActionCreators";
import ModalContent from "../components/Modal/ModalContent";
import ModalFooter from "../components/Modal/ModalFooter";
import ModalButton from "../components/Modal/ModalButton";

function ProfileDeleteModule() {
    const [confirmModalActive, setConfirmModalActive] = useState(false);
    const [isConfirmed, setConfirmed] = useState(false);
    const [isSubmitted, setSubmitted] = useState(false);

    const { user } = useAppSelector(state => state.userReducer);
    const { accessToken } = useAppSelector(state => state.authReducer);
    const dispatch = useAppDispatch();
    const { removeUserQuery, error, success, resetValues } = useRemoveUser();

    useEffect(() => {
        if (success) {
            dispatch(logout());
            dispatch(resetUserState());
        }
    }, [success]);

    async function onSubmit() {
        setSubmitted(true);

        if (isConfirmed) {
            closeModal();
            if (user && accessToken) {
                await removeUserQuery(user.id, accessToken);
            }
        }
    }

    function closeModal() {
        setConfirmModalActive(false);
        setConfirmed(false);
        setSubmitted(false);
    }
    return (
        <>
            <h3 className="account-page__title">Удалить профиль</h3>

            <div className="indented">
                <div className="text-danger">
                    Вы действительно хотите удалить профиль?
                    <br />
                    Все данные будут безвозвратно удалены!
                </div>

                <button className="button-danger" onClick={() => setConfirmModalActive(true)}>Удалить</button>
            </div>

            <Modal
                active={confirmModalActive}
                title="Подтвердите действие"
                onClose={closeModal}>

                <ModalContent>
                    <div className="indented">
                        {isSubmitted && !isConfirmed &&
                            <p className="error-text required">Вы не подтвердили действие</p>
                        }
                        <Checkbox
                            checkedChanger={() => { setConfirmed(prev => !prev) }}
                            isChecked={isConfirmed}
                            labelClassName="ui-text"
                            label="Вы подтверждаете удаление вашего профиля и всех ваших данных внутри курсов, включая прогрессы прохождения курса, модулей, уроков и т.д." />
                    </div>
                </ModalContent>

                <ModalFooter>
                    <ModalButton text="Подтвердить" onClick={onSubmit} />
                </ModalFooter>
            </Modal>

            {error &&
                <PopupNotification notificationType={NotificationType.error} onFade={resetValues}>
                    {error}
                </PopupNotification>
            }
        </>
    );
}

export default ProfileDeleteModule;