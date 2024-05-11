import {Participant} from "../../models/participant.ts";
import {useKick} from "../../hooks/subscriptionHooks.ts";
import {Modal} from "../../components/Modal/Modal.tsx";
import ModalFooter from "../../components/Modal/ModalFooter.tsx";
import ModalButton from "../../components/Modal/ModalButton.tsx";
import PopupNotification from "../../components/PopupNotification.tsx";
import {NotificationType} from "../../models/enums.ts";
import PopupLoader from "../../components/Loader/PopupLoader.tsx";

interface IParticipantKickModalProps {
    requesterUserId: number;
    accessToken: string;
    participant: Participant;
    courseId: number;
    active: boolean;
    onClose: () => void;
}

function ParticipantKickModal({
  requesterUserId,
  accessToken,
  participant,
  courseId,
  active,
  onClose}: IParticipantKickModalProps) {

    const { kickQuery, error, success, loading, resetValues } = useKick();

    async function onSubmit() {
        await kickQuery(requesterUserId, participant.id, courseId, accessToken);
    }

    function closeModal() {
        resetValues();
        onClose();
    }

    return (
        <>
            {!error && !loading && !success &&
                <Modal active={active} onClose={closeModal} title="Исключить пользователя">
                    <div className="indented">
                        Подтвердить исключение пользователя <span className="text-violet">{participant.nickname}</span>
                    </div>
                    <ModalFooter>
                        <ModalButton onClick={onSubmit} text="Исключить" />
                    </ModalFooter>
                </Modal>
            }

            {error &&
                <PopupNotification notificationType={NotificationType.error} onFade={closeModal}>
                    {error}
                </PopupNotification>
            }

            {success &&
                <PopupNotification notificationType={NotificationType.success} onFade={closeModal}>
                    {success}
                </PopupNotification>
            }

            {loading &&
                <PopupLoader />
            }

        </>

    )
}

export default ParticipantKickModal;