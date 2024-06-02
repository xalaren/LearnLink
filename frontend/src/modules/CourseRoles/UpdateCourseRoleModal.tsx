import { Modal } from "../../components/Modal/Modal.tsx";
import { useState } from "react";
import { LocalRole } from "../../models/localRole.ts";
import { validate } from "../../helpers/validation.ts";
import { Input } from "../../components/Input/Input.tsx";
import { InputType } from "../../models/enums.ts";
import ModalContent from "../../components/Modal/ModalContent.tsx";
import ModalButton from "../../components/Modal/ModalButton.tsx";
import Switch from "../../components/Switch.tsx";
import { useRequestUpdateCourseLocalRole } from "../../hooks/courseLocalRoleHooks.ts";
import PopupLoader from "../../components/Loader/PopupLoader.tsx";
import { ErrorModal } from "../../components/Modal/ErrorModal.tsx";
import { SuccessModal } from "../../components/Modal/SuccessModal.tsx";
import ModalFooter from "../../components/Modal/ModalFooter.tsx";

interface IUpdateLocalRoleModal {
    active: boolean;
    userId: number;
    courseId: number;
    accessToken: string;
    localRole: LocalRole;
    onClose: () => void;
}

function UpdateLocalRoleModal({
    active,
    userId,
    courseId,
    accessToken,
    localRole,
    onClose
}: IUpdateLocalRoleModal) {
    const [name, setName] = useState(localRole.name);
    const [sign, setSign] = useState(localRole.sign);
    const [nameError, setNameError] = useState('');
    const [signError, setSignError] = useState('');

    const [viewAccess, setViewAccess] = useState(localRole.viewAccess);
    const [editAccess, setEditAccess] = useState(localRole.editAccess);
    const [removeAccess, setRemoveAccess] = useState(localRole.removeAccess);
    const [manageInternalAccess, setManageInternalAccess] = useState(localRole.manageInternalAccess);
    const [kickAccess, setKickAccess] = useState(localRole.kickAccess);
    const [inviteAccess, setInviteAccess] = useState(localRole.inviteAccess);
    const [editRolesAccess, setEditRolesAccess] = useState(localRole.editRolesAccess);

    const { requestUpdateCourseLocalRoleQuery, loading, error, success, resetValues } = useRequestUpdateCourseLocalRole();

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

        const regex: RegExp = /^[a-z]+$/gm;
        if (!regex.test(sign)) {
            setSignError('Сигнатура должна состоять из латинских символов нижнего регистра');
            isValidated = false;
        }

        if (!isValidated) return;

        const newLocalRole = new LocalRole(
            localRole.id,
            name,
            sign,
            viewAccess,
            editAccess,
            removeAccess,
            manageInternalAccess,
            inviteAccess,
            kickAccess,
            editRolesAccess);

        await requestUpdateCourseLocalRoleQuery(userId, courseId, newLocalRole, accessToken);
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
                <Modal active={active} onClose={closeModal} title="Создание локальной роли">
                    <ModalContent>
                        <form className="form">
                            <div className="form__inputs">
                                <Input
                                    type={InputType.text}
                                    name="name"
                                    label="Название роли"
                                    placeholder="Введите название..."
                                    errorMessage={nameError}
                                    required={true}
                                    value={name}
                                    onChange={onChange}
                                />

                                <Input
                                    type={InputType.text}
                                    name="sign"
                                    label="Сигнатура роли"
                                    placeholder="Введите сигнатуру (только латинские буквы)..."
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
                                        checkedChanger={() => setEditAccess(prev => !prev)}
                                        label="Разрешить редактирование курса"
                                    />
                                    <Switch
                                        isChecked={removeAccess}
                                        checkedChanger={() => setRemoveAccess(prev => !prev)}
                                        label="Разрешить удаление курса"
                                    />
                                    <Switch
                                        isChecked={manageInternalAccess}
                                        checkedChanger={() => setManageInternalAccess(prev => !prev)}
                                        label="Разрешить редактирование материалов курса"
                                    />
                                    <Switch
                                        isChecked={inviteAccess}
                                        checkedChanger={() => setInviteAccess(prev => !prev)}
                                        label="Разрешить приглашение пользователей"
                                    />
                                    <Switch
                                        isChecked={kickAccess}
                                        checkedChanger={() => setKickAccess(prev => !prev)}
                                        label="Разрешить исключение пользователей"
                                    />
                                    <Switch
                                        isChecked={editRolesAccess}
                                        checkedChanger={() => setEditRolesAccess(prev => !prev)}
                                        label="Разрешить редактировать роли"
                                    />
                                </div>

                            </div>
                        </form>
                    </ModalContent>
                    <ModalFooter>
                        <ModalButton text="Сохранить" onClick={onSubmit} />
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

export default UpdateLocalRoleModal;