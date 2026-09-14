import { useContext, useState } from "react";
import Checkbox from "../../components/Checkbox";
import PopupLoader from "../../components/Loader/PopupLoader";
import { Modal } from "../../components/Modal/Modal";
import ModalButton from "../../components/Modal/ModalButton";
import ModalContent from "../../components/Modal/ModalContent";
import ModalFooter from "../../components/Modal/ModalFooter";
import { SuccessModal } from "../../components/Modal/SuccessModal";
import PopupNotification from "../../components/PopupNotification";
import { useHistoryNavigation } from "../../hooks/historyNavigation";
import { useAppSelector } from "../../hooks/redux";
import { NotificationType } from "../../models/enums";
import { paths } from "../../models/paths";
import { ModuleContext } from "../../contexts/ModuleContext";
import { useRemoveModule } from "../../hooks/moduleHooks";

interface IModuleDeleteModalProps {
    active: boolean;
    courseId: number;
    moduleId: number;
    onClose: () => void;
}

function ModuleDeleteModal({ active, moduleId, courseId, onClose }: IModuleDeleteModalProps) {
    const [isConfirmed, setConfirmed] = useState(false);
    const [isSubmitted, setSubmitted] = useState(false);

    const { user } = useAppSelector(state => state.userReducer);
    const { accessToken } = useAppSelector(state => state.authReducer);
    const { toNext } = useHistoryNavigation();

    const { fetchModule } = useContext(ModuleContext);

    const { removeModuleQuery, loading, error, success, resetValues } = useRemoveModule();

    function closeModal() {
        resetDefault();
        fetchModule();
        onClose();
    }

    function resetDefault() {
        setConfirmed(false);
        setSubmitted(false);
        resetValues();
    }

    async function onDeleteSubmit() {
        setSubmitted(true);

        if (isConfirmed) {
            if (user && accessToken) {
                await removeModuleQuery(moduleId, courseId, user.id, accessToken);
            }

        }
    }



    return (
        <>
            {!error && !loading &&
                <Modal
                    active={active && !success}
                    title="Подтвердите действие"
                    onClose={onClose}>

                    <ModalContent className="indented">
                        {isSubmitted && !isConfirmed &&
                            <p className="error-text required">Вы не подтвердили действие</p>
                        }
                        <Checkbox
                            checkedChanger={() => { setConfirmed(prev => !prev) }}
                            isChecked={isConfirmed}
                            labelClassName="ui-text"
                            label="Вы подтверждаете удаление модуля? Это повлечет удаление всех уроков, а также прогрессов внутри этого модуля!" />
                    </ModalContent>

                    <ModalFooter>
                        <ModalButton className="button-danger-light" text="Удалить модуль" onClick={onDeleteSubmit} />
                    </ModalFooter>

                </Modal>
            }

            {loading &&
                <PopupLoader />
            }

            {error &&
                <PopupNotification notificationType={NotificationType.error} onFade={closeModal}>
                    {error}
                </PopupNotification>
            }

            {success &&
                <SuccessModal active={Boolean(success)} message="Модуль успешно удален" onClose={() => toNext(paths.course.view.full(courseId))}>
                </SuccessModal>
            }
        </>
    );
}

export default ModuleDeleteModal;