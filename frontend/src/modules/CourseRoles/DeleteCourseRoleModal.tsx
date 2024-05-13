import { Modal } from "../../components/Modal/Modal.tsx";
import { useState } from "react";
import { LocalRole } from "../../models/localRole.ts";
import { validate } from "../../helpers/validation.ts";
import { Input } from "../../components/Input.tsx";
import { InputType } from "../../models/enums.ts";
import ModalContent from "../../components/Modal/ModalContent.tsx";
import ModalButton from "../../components/Modal/ModalButton.tsx";
import Switch from "../../components/Switch.tsx";
import { useRequestDeleteCourseLocalRole, useRequestUpdateCourseLocalRole } from "../../hooks/courseLocalRoleHooks.ts";
import PopupLoader from "../../components/Loader/PopupLoader.tsx";
import { ErrorModal } from "../../components/Modal/ErrorModal.tsx";
import { SuccessModal } from "../../components/Modal/SuccessModal.tsx";
import ModalFooter from "../../components/Modal/ModalFooter.tsx";

interface IDeleteCourseLocalRoleModalProps {
    active: boolean;
    userId: number;
    courseId: number;
    accessToken: string;
    localRole: LocalRole;
    onClose: () => void;
}

function DeleteCourseLocalRoleModal({
    active,
    userId,
    courseId,
    accessToken,
    localRole,
    onClose
}: IDeleteCourseLocalRoleModalProps) {

    const { requestDeleteCourseLocalRoleQuery, loading, error, success, resetValues } = useRequestDeleteCourseLocalRole();

    async function onSubmit() {
        await requestDeleteCourseLocalRoleQuery(userId, courseId, localRole.id, accessToken);
    }

    function closeModal() {
        resetValues();
        onClose();
    }

    return (
        <>
            {!error && !loading && !success &&
                <Modal active={active} onClose={closeModal} title="Подтвердите удаление локальной роли">
                    <ModalContent className="indented">
                        Подтвердить удаление локальной роли "<span className="text-violet">{localRole.name}</span>"?
                    </ModalContent>
                    <ModalFooter>
                        <ModalButton text="Подтвердить" onClick={onSubmit} />
                        <ModalButton text="Отмена" onClick={closeModal} />
                    </ModalFooter>
                </Modal>
            }

            {!error && loading &&
                <PopupLoader />
            }

            {error &&
                <ErrorModal active={Boolean(error)} onClose={closeModal} error={error} />
            }

            {success &&
                <SuccessModal active={Boolean(success)} onClose={closeModal} message={success} />
            }
        </>
    );
}

export default DeleteCourseLocalRoleModal;