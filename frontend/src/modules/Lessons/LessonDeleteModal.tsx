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
import { LessonContext } from "../../contexts/LessonContext";
import { useRemoveLesson } from "../../hooks/lessonHook";

interface IModuleDeleteModalProps {
    active: boolean;
    courseId: number;
    moduleId: number;
    lessonId: number;
    onClose: () => void;
}

function ModuleDeleteModal({ active, moduleId, courseId, lessonId, onClose }: IModuleDeleteModalProps) {
    const [isConfirmed, setConfirmed] = useState(false);
    const [isSubmitted, setSubmitted] = useState(false);

    const { user } = useAppSelector(state => state.userReducer);
    const { accessToken } = useAppSelector(state => state.authReducer);
    const { toNext } = useHistoryNavigation();

    const { fetchLesson } = useContext(LessonContext);

    const { removeLessonQuery, success, error, loading, resetValues } = useRemoveLesson();

    function closeModal() {
        resetDefault();
        fetchLesson();
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
                await removeLessonQuery(user.id, courseId, moduleId, lessonId, accessToken);
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
                            label="Вы подтверждаете удаление урока?" />
                    </ModalContent>

                    <ModalFooter>
                        <ModalButton className="button-danger-light" text="Удалить урок" onClick={onDeleteSubmit} />
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
                <SuccessModal active={Boolean(success)} message="Урок успешно удален" onClose={() => toNext(paths.module.view.full(courseId, moduleId))}>
                </SuccessModal>
            }
        </>
    );
}

export default ModuleDeleteModal;