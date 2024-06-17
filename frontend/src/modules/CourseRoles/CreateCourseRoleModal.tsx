import { Modal } from "../../components/Modal/Modal.tsx";
import { useState } from "react";
import { LocalRole } from "../../models/localRole.ts";
import { validate } from "../../helpers/validation.ts";
import { Input } from "../../components/Input/Input.tsx";
import { InputType } from "../../models/enums.ts";
import ModalContent from "../../components/Modal/ModalContent.tsx";
import ModalButton from "../../components/Modal/ModalButton.tsx";
import Switch from "../../components/Switch.tsx";
import { useRequestCreateCourseLocalRole } from "../../hooks/courseLocalRoleHooks.ts";
import PopupLoader from "../../components/Loader/PopupLoader.tsx";
import { ErrorModal } from "../../components/Modal/ErrorModal.tsx";
import { SuccessModal } from "../../components/Modal/SuccessModal.tsx";

interface ICourseRoleModalProps {
    active: boolean;
    userId: number;
    courseId: number;
    accessToken: string;
    onClose: () => void;
}

function CreateCourseRoleModal({
    active,
    userId,
    courseId,
    accessToken,
    onClose
}: ICourseRoleModalProps) {
    const [name, setName] = useState('');
    const [sign, setSign] = useState('');
    const [nameError, setNameError] = useState('');
    const [signError, setSignError] = useState('');

    const [viewAccess, setViewAccess] = useState(false);
    const [editAccess, setEditAccess] = useState(false);
    const [removeAccess, setRemoveAccess] = useState(false);
    const [manageInternalAccess, setManageInternalAccess] = useState(false);
    const [kickAccess, setKickAccess] = useState(false);
    const [inviteAccess, setInviteAccess] = useState(false);
    const [editRolesAccess, setEditRolesAccess] = useState(false);

    const isModerator =
        viewAccess &&
        editAccess &&
        removeAccess &&
        manageInternalAccess &&
        inviteAccess &&
        kickAccess &&
        editRolesAccess;

    const { requestCreateCourseLocalRoleQuery, loading, error, success, resetValues } = useRequestCreateCourseLocalRole();

    function onChange(event: React.ChangeEvent) {
        const inputElement = event.target as HTMLInputElement;

        switch (inputElement.name) {
            case 'name':
                setName(inputElement.value);
                setNameError('');
                break;
            case 'sign':
                setSign(inputElement.value);
                setSignError('');
                break;
        }
    }

    async function onSubmit() {
        let isValidated: boolean = true;

        if (!validate(name)) {
            setNameError('Название локальной роли должно быть введено');
            isValidated = false;
        }

        const regex: RegExp = /^^[a-zA-Z][a-zA-Z0-9_]*$/gm;;
        if (!regex.test(sign)) {
            setSignError('Сигнатура должна состоять из латинских символов и не может начинаться с цифр или других символов');
            isValidated = false;
        }

        if (!isValidated) return;

        const localRole = new LocalRole(
            0,
            name,
            sign,
            viewAccess,
            editAccess,
            removeAccess,
            manageInternalAccess,
            inviteAccess,
            kickAccess,
            editRolesAccess);

        await requestCreateCourseLocalRoleQuery(userId, courseId, localRole, accessToken);
    }

    function closeModal() {
        setName('');
        setSign('');
        resetValues();
        onClose();
    }

    return (
        <>
            {!error && !loading && !success &&
                <Modal
                    active={active}
                    onClose={closeModal}
                    title="Создание локальной роли"
                    contentClassName="modal__content-restricted">
                    <ModalContent>
                        <form className="form">
                            <div className="form__inputs">
                                <Input
                                    type={InputType.text}
                                    name="name"
                                    label="Название роли"
                                    placeholder="Введите название..."
                                    required={true}
                                    errorMessage={nameError}
                                    value={name}
                                    onChange={onChange}
                                />

                                <Input
                                    type={InputType.text}
                                    name="sign"
                                    label="Сигнатура роли"
                                    placeholder="Введите сигнатуру (только латиница)..."
                                    required={true}
                                    errorMessage={signError}
                                    value={sign}
                                    onChange={onChange}
                                />

                                <div className="checkbox-list">
                                    <Switch
                                        isChecked={viewAccess}
                                        checkedChanger={() => setViewAccess(prev => !prev)}
                                        label="Разрешить просмотр курса"
                                    />
                                    <Switch
                                        isChecked={editAccess}
                                        checkedChanger={() => {
                                            setEditAccess(prev => !prev);
                                            setViewAccess(true);
                                        }}
                                        label="Разрешить редактирование курса"
                                    />
                                    <Switch
                                        isChecked={removeAccess}
                                        label="Разрешить удаление курса"
                                        checkedChanger={() => {
                                            setRemoveAccess(prev => !prev);
                                            setViewAccess(true);
                                        }}
                                    />
                                    <Switch
                                        isChecked={manageInternalAccess}
                                        label="Разрешить редактирование материалов курса"
                                        checkedChanger={() => {
                                            setManageInternalAccess(prev => !prev);
                                            setViewAccess(true);
                                        }}
                                    />
                                    <Switch
                                        isChecked={inviteAccess}
                                        label="Разрешить приглашение пользователей"
                                        checkedChanger={() => {
                                            setInviteAccess(prev => !prev);
                                            setViewAccess(true);
                                        }}
                                    />
                                    <Switch
                                        isChecked={kickAccess}
                                        label="Разрешить исключение пользователей"
                                        checkedChanger={() => {
                                            setKickAccess(prev => !prev);
                                            setViewAccess(true);
                                        }}
                                    />
                                    <Switch
                                        isChecked={editRolesAccess}
                                        label="Разрешить редактировать роли"
                                        checkedChanger={() => {
                                            setEditRolesAccess(prev => !prev);
                                            setViewAccess(true);
                                        }}
                                    />
                                    <Switch
                                        isChecked={isModerator}
                                        label="Модератор курса"
                                        checkedChanger={() => {
                                            if (isModerator) {
                                                setViewAccess(false);
                                                setEditAccess(false);
                                                setRemoveAccess(false);
                                                setManageInternalAccess(false);
                                                setInviteAccess(false);
                                                setKickAccess(false);
                                                setEditRolesAccess(false);
                                            }
                                            else {
                                                setViewAccess(true);
                                                setEditAccess(true);
                                                setRemoveAccess(true);
                                                setManageInternalAccess(true);
                                                setInviteAccess(true);
                                                setKickAccess(true);
                                                setEditRolesAccess(true);
                                            }
                                        }}
                                    />
                                </div>

                            </div>
                        </form>
                    </ModalContent>
                    <ModalButton text="Создать" onClick={onSubmit} />
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

export default CreateCourseRoleModal;