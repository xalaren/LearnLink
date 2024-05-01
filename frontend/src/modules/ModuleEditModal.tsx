/* eslint-disable react-hooks/exhaustive-deps */
import { useEffect, useState } from "react";
import { Modal } from "../components/Modal";
import { validate } from "../helpers/validation";
import { Input } from "../components/Input";
import { InputType, NotificationType } from "../models/enums";
import PopupLoader from "../components/PopupLoader";
import Notification from "../components/PopupNotification";
import { useAppSelector } from "../hooks/redux";
import { Module } from "../models/module";
import { useUpdateModule } from "../hooks/moduleHooks";

interface IModuleEditModalProps {
    active: boolean;
    defaultModule: Module;
    onClose: () => void;
    refreshRequest: () => void;
}

function ModuleEditModal({ active, defaultModule, onClose, refreshRequest }: IModuleEditModalProps) {
    const { updateModuleQuery, success, error, loading, resetValues } = useUpdateModule();

    const [moduleTitle, setModuleTitle] = useState('');
    const [moduleTitleError, setModuleTitleError] = useState('');

    const [moduleDescription, setModuleDescription] = useState('');

    const { user } = useAppSelector(state => state.userReducer);
    const { accessToken } = useAppSelector(state => state.authReducer);

    useEffect(() => {
        resetInputs();
    }, [defaultModule.description, defaultModule.title]);


    function resetInputs() {
        setModuleTitle(defaultModule.title);
        setModuleTitleError('');
        setModuleDescription(defaultModule.description || '');
    }

    async function onSubmit(event: React.FormEvent) {
        event.preventDefault();

        let isValidDate = true;


        if (!validate(moduleTitle)) {
            setModuleTitleError("Название курса должно быть заполнено");
            isValidDate = false;
        }
        if (!isValidDate) return;

        if (accessToken && user) {
            await updateModuleQuery(defaultModule.id, moduleTitle, accessToken, moduleDescription);
            refreshRequest();
        }

    }

    function onChange(event: React.ChangeEvent) {
        const inputElement = event.target as HTMLInputElement;

        switch (inputElement.name) {
            case 'moduleTitle':
                setModuleTitle(inputElement.value);
                setModuleTitleError('');
                break;
            case 'courseDescription':
                setModuleDescription(inputElement.value);
                break;
        }
    }

    return (
        <>
            <Modal active={active} title="Редактирование модуля" onClose={() => {
                resetInputs();
                onClose();
            }}>
                <form className="base-form" onSubmit={onSubmit}>
                    <ul className="form__inputs">
                        <p className="medium-little">
                            Название модуля
                        </p>
                        <Input
                            type={InputType.text}
                            name="courseTitle"
                            onChange={onChange}
                            errorMessage={moduleTitleError}
                            placeholder="Введите название модуля..."
                            width={500}
                            value={moduleTitle}
                        />

                        <p className="medium-little">Описание модуля</p>
                        <Input
                            type={InputType.rich}
                            name="courseDescription"
                            onChange={onChange}
                            placeholder="Введите описание модуля (Необязательно)..."
                            styles={{ width: '500px' }}
                            value={moduleDescription}
                        />
                    </ul>
                    <nav className="form__nav">
                        <button className="button-violet" type="submit">Сохранить</button>
                    </nav>
                </form>
            </Modal >

            {loading && <PopupLoader />}
            {error && <Notification notificationType={NotificationType.error} onFade={resetValues}>{error}</Notification>}
            {success && <Notification notificationType={NotificationType.success} onFade={resetValues}>{success}</Notification>}
        </>
    );
}

export default ModuleEditModal;