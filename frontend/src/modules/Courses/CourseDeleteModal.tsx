import { useState } from "react";
import Checkbox from "../../components/Checkbox";
import PopupLoader from "../../components/Loader/PopupLoader";
import { Modal } from "../../components/Modal/Modal";
import ModalButton from "../../components/Modal/ModalButton";
import ModalContent from "../../components/Modal/ModalContent";
import ModalFooter from "../../components/Modal/ModalFooter";
import { SuccessModal } from "../../components/Modal/SuccessModal";
import PopupNotification from "../../components/PopupNotification";
import { useHideCourse, useRemoveCourse } from "../../hooks/courseHooks";
import { useHistoryNavigation } from "../../hooks/historyNavigation";
import { useAppSelector } from "../../hooks/redux";
import { NotificationType, ViewTypes } from "../../models/enums";
import { paths } from "../../models/paths";

interface ICourseDeleteModalProps {
    active: boolean;
    courseId: number;
    onClose: () => void;
}

function CourseDeleteModal({ active, courseId, onClose }: ICourseDeleteModalProps) {
    const [isConfirmed, setConfirmed] = useState(false);
    const [isSubmitted, setSubmitted] = useState(false);
    const [error, setError] = useState('');

    const { user } = useAppSelector(state => state.userReducer);
    const { accessToken } = useAppSelector(state => state.authReducer);
    const { toNext } = useHistoryNavigation();

    const removeCourseHook = useRemoveCourse();
    const hideCourseHook = useHideCourse();



    function closeModal() {
        resetDefault();
        onClose();
    }

    function resetDefault() {
        setConfirmed(false);
        setSubmitted(false);
        removeCourseHook.resetValues();
        hideCourseHook.resetValues();
    }

    async function onDeleteSubmit() {
        setSubmitted(true);

        if (isConfirmed) {
            if (user && accessToken) {
                await removeCourseHook.removeCourseQuery(courseId, user.id, accessToken);
            }

            if (removeCourseHook.error) {
                setError(removeCourseHook.error);
                return;
            }

        }
    }

    async function onHideSubmit() {
        if (user && accessToken) {
            await hideCourseHook.hideCourseQuery(courseId, user.id, accessToken);
        }

        if (hideCourseHook.error) {
            setError(hideCourseHook.error);
            return;
        }
    }

    return (
        <>
            <Modal
                active={active && !removeCourseHook.success}
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
                        label="Вы подтверждаете удаление курсов? Это повлечет удаление всех модулей, уроков, а также прогрессов, сделанных участниками этого курса.Рекомендуем скрыть курс вместо полного удаления!" />
                </ModalContent>

                <ModalFooter>
                    <ModalButton text="Скрыть курс" onClick={onHideSubmit} />
                    <ModalButton className="button-danger-light" text="Удалить курс" onClick={onDeleteSubmit} />
                </ModalFooter>

            </Modal>

            {removeCourseHook.loading || hideCourseHook.loading &&
                <PopupLoader />
            }

            {error &&
                <PopupNotification notificationType={NotificationType.error} onFade={closeModal}>
                    {error}
                </PopupNotification>
            }

            {removeCourseHook.success &&
                <SuccessModal active={Boolean(removeCourseHook.success)} message="Курс успешно удален" onClose={() => toNext(paths.profile.courses(ViewTypes.created))}>
                </SuccessModal>
            }

            {hideCourseHook.success &&
                <PopupNotification notificationType={NotificationType.success} onFade={resetDefault}>
                    {hideCourseHook.success}
                </PopupNotification>
            }
        </>
    );
}

export default CourseDeleteModal;