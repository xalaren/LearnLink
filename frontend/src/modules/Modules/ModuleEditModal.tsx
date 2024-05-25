import { useContext, useEffect, useState } from "react";
import { CourseContext } from "../../contexts/CourseContext";
import { ModuleContext } from "../../contexts/ModuleContext";
import { useAppSelector } from "../../hooks/redux";
import { validate } from "../../helpers/validation";
import { Modal } from "../../components/Modal/Modal";
import ModalContent from "../../components/Modal/ModalContent";
import { Input } from "../../components/Input";
import { InputType, NotificationType } from "../../models/enums";
import ModalFooter from "../../components/Modal/ModalFooter";
import ModalButton from "../../components/Modal/ModalButton";
import PopupLoader from "../../components/Loader/PopupLoader";
import PopupNotification from "../../components/PopupNotification";
import { useUpdateModule } from "../../hooks/moduleHooks";

interface IModuleEditModalProps {
    active: boolean;
    onClose: () => void;
}

function ModuleEditModal({ active, onClose }: IModuleEditModalProps) {
    const { course } = useContext(CourseContext);
    const { module, fetchModule } = useContext(ModuleContext);

    const [title, setTitle] = useState('');
    const [description, setDescription] = useState('');
    const [titleError, setTitleError] = useState('');

    const { accessToken } = useAppSelector(state => state.authReducer);
    const { user } = useAppSelector(state => state.userReducer);
    const { updateModuleQuery, loading, error, success, resetValues } = useUpdateModule();

    useEffect(() => {
        if (module) {
            setTitle(module.title);
            setDescription(module.description || '');
        }
    }, [user])

    function closeModal() {
        fetchModule();
        resetValues();
        onClose();
    }

    function onChange(event: React.ChangeEvent) {
        const inputElement = event.target as HTMLInputElement;

        switch (inputElement.name) {
            case 'title':
                setTitle(inputElement.value);
                setTitleError('');
                break;
            case 'description':
                setDescription(inputElement.value);
                break;
        }
    }

    async function updateModule() {
        let isValidated = true;

        if (!validate(title)) {
            setTitleError('Название модуля должно быть заполнено')
            isValidated = false;
        }

        if (isValidated && user && accessToken && course && module) {
            const newModule = { ...module, title, description };

            await updateModuleQuery(newModule, course.id, user.id, accessToken);
        }
    }

    return (
        <>
            {!error && !loading && !success &&
                <Modal
                    active={active}
                    onClose={closeModal}
                    title="Редактирование модуля">

                    <ModalContent>
                        <form className="form">
                            <div className="form__inputs">
                                <Input
                                    type={InputType.text}
                                    name="title"
                                    label="Название модуля"
                                    placeholder="Введите название..."
                                    errorMessage={titleError}
                                    value={title}
                                    onChange={onChange}
                                />

                                <Input
                                    type={InputType.rich}
                                    name="description"
                                    label="Описание модуля"
                                    placeholder="Введите описание (необязательно)..."
                                    value={description}
                                    onChange={onChange}
                                />
                            </div>
                        </form>
                    </ModalContent>

                    <ModalFooter>
                        <ModalButton text="Сохранить" onClick={updateModule} />
                    </ModalFooter>

                </Modal >
            }

            {loading &&
                <PopupLoader />
            }

            {success &&
                <PopupNotification notificationType={NotificationType.success} onFade={closeModal}>
                    {success}
                </PopupNotification>
            }

            {error &&
                <PopupNotification notificationType={NotificationType.error} onFade={closeModal}>
                    {error}
                </PopupNotification>
            }
        </>
    );

}

export default ModuleEditModal;