import { useContext, useState } from "react";
import { useHistoryNavigation } from "../../hooks/historyNavigation";
import { useAppSelector } from "../../hooks/redux";
import { LessonContext } from "../../contexts/LessonContext";
import { Modal } from "../../components/Modal/Modal";
import ModalContent from "../../components/Modal/ModalContent";
import Checkbox from "../../components/Checkbox";
import ModalFooter from "../../components/Modal/ModalFooter";
import ModalButton from "../../components/Modal/ModalButton";
import PopupLoader from "../../components/Loader/PopupLoader";
import PopupNotification from "../../components/PopupNotification";
import { NotificationType } from "../../models/enums";
import { SuccessModal } from "../../components/Modal/SuccessModal";
import { paths } from "../../models/paths";
import { useAnswerQueries } from "../../hooks/answerHooks";

interface IAnswerDeleteModalProps {
    active: boolean;
    courseId: number;
    moduleId: number;
    lessonId: number;
    objectiveId: number;
    answerId: number;
    onClose: () => void;
}

function AnswerDeleteModal({
    active,
    courseId,
    moduleId,
    lessonId,
    objectiveId,
    answerId,
    onClose }: IAnswerDeleteModalProps) {
    const [isConfirmed, setConfirmed] = useState(false);
    const [isSubmitted, setSubmitted] = useState(false);

    const { accessToken } = useAppSelector(state => state.authReducer);
    const { toNext } = useHistoryNavigation();

    const { fetchLesson } = useContext(LessonContext);
    const { removeQuery, resetValues, error, success, loading } = useAnswerQueries();

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
            if (accessToken) {
                await removeQuery(lessonId, answerId, accessToken);
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
                            label="Вы подтверждаете удаление ответа?" />
                    </ModalContent>

                    <ModalFooter>
                        <ModalButton className="button-danger-light" text="Удалить ответ на задание" onClick={onDeleteSubmit} />
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
                <SuccessModal active={Boolean(success)} message={success} onClose={() =>
                    toNext(paths.objective.view.full(courseId, moduleId, lessonId, objectiveId))}>
                </SuccessModal>
            }
        </>
    );
}

export default AnswerDeleteModal;