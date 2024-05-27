import { useEffect, useState } from "react";
import { Input } from "../../components/Input";
import PopupLoader from "../../components/Loader/PopupLoader";
import { Modal } from "../../components/Modal/Modal";
import ModalButton from "../../components/Modal/ModalButton";
import ModalContent from "../../components/Modal/ModalContent";
import ModalFooter from "../../components/Modal/ModalFooter";
import PopupNotification from "../../components/PopupNotification";
import { validate } from "../../helpers/validation";
import { InputType, NotificationType } from "../../models/enums";
import { useAppSelector } from "../../hooks/redux";
import { useCreateLesson } from "../../hooks/lessonHook";

interface ILessonCreateModalProps {
    courseId: number;
    moduleId: number;
    active: boolean;
    onClose: () => void;

}

function ModuleCreateModal({ courseId, moduleId, active, onClose }: ILessonCreateModalProps) {
    const [title, setTitle] = useState('');
    const [description, setDescription] = useState('');
    const [titleError, setTitleError] = useState('');

    const { user } = useAppSelector(state => state.userReducer);
    const { accessToken } = useAppSelector(state => state.authReducer);
    const { createLessonQuery, success, error, loading, resetValues } = useCreateLesson();

    useEffect(() => {
    }, [accessToken]);

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

    async function createModule() {
        let isValidated = true;

        if (!validate(title)) {
            setTitleError('Название модуля должно быть заполнено')
            isValidated = false;
        }

        if (isValidated && user && accessToken) {
            await createLessonQuery(user.id, courseId, moduleId, title, accessToken, description);
        }
    }

    function resetDefault() {
        resetValues();
        setTitle('');
        setDescription('');
        setTitleError('');
    }

    function closeModal() {
        resetDefault();
        onClose();
    }

    return (
        <>
            {!loading && !error && !success &&
                <Modal
                    active={active}
                    onClose={closeModal}
                    title="Создание урока">

                    <ModalContent>
                        <form className="form">
                            <div className="form__inputs">
                                <Input
                                    type={InputType.text}
                                    name="title"
                                    label="Название урока"
                                    placeholder="Введите название..."
                                    errorMessage={titleError}
                                    required={true}
                                    value={title}
                                    onChange={onChange}
                                />

                                <Input
                                    type={InputType.rich}
                                    name="description"
                                    label="Описание урока"
                                    placeholder="Введите описание (необязательно)..."
                                    value={description}
                                    onChange={onChange}
                                />
                            </div>
                        </form>
                    </ModalContent>

                    <ModalFooter>
                        <ModalButton text="Сохранить" onClick={createModule} />
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

export default ModuleCreateModal;