import { useEffect, useState } from "react";
import Checkbox from "../../components/Checkbox";
import PopupLoader from "../../components/Loader/PopupLoader";
import { Modal } from "../../components/Modal/Modal";
import ModalButton from "../../components/Modal/ModalButton";
import ModalContent from "../../components/Modal/ModalContent";
import ModalFooter from "../../components/Modal/ModalFooter";
import PopupNotification from "../../components/PopupNotification";
import { InputType, NotificationType } from "../../models/enums";
import { useAppSelector } from "../../hooks/redux";
import { useCreateCourse } from "../../hooks/courseHooks";
import { validate } from "../../helpers/validation";
import { Input } from "../../components/Input/Input";
import RoundedCheckbox from "../../components/LoadedCheckbox";

interface ICourseCreateModalProps {
    active: boolean;
    onClose: () => void;
}

function CourseCreateModal({ active, onClose }: ICourseCreateModalProps) {
    const [title, setTitle] = useState('');
    const [description, setDescription] = useState('');
    const [isPublic, setPublic] = useState(false);
    const [titleError, setTitleError] = useState('');

    const { accessToken } = useAppSelector(state => state.authReducer);
    const { user } = useAppSelector(state => state.userReducer);
    const { createCourseQuery, loading, error, success, resetValues } = useCreateCourse();

    useEffect(() => {
    }, [user])

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

    async function createCourse() {
        let isValidated = true;

        if (!validate(title)) {
            setTitleError('Название курса должно быть заполнено')
            isValidated = false;
        }

        if (isValidated && user && accessToken) {
            await createCourseQuery(title, isPublic, user.id, accessToken, description);
        }
    }

    function resetDefault() {
        resetValues();
        setTitle('');
        setDescription('');
        setTitleError('');
        setPublic(false);
    }

    function closeModal() {
        resetDefault();
        onClose();
    }

    return (
        <>
            {!error && !loading && !success &&
                <Modal
                    active={active}
                    onClose={closeModal}
                    title="Создание курса">

                    <ModalContent>
                        <form className="form">
                            <div className="form__inputs">
                                <Input
                                    type={InputType.text}
                                    name="title"
                                    label="Название курса"
                                    placeholder="Введите название..."
                                    required={true}
                                    errorMessage={titleError}
                                    value={title}
                                    onChange={onChange}
                                />

                                <Input
                                    type={InputType.rich}
                                    name="description"
                                    label="Описание курса"
                                    placeholder="Введите описание (необязательно)..."
                                    value={description}
                                    onChange={onChange}
                                />

                                <Checkbox
                                    isChecked={isPublic}
                                    checkedChanger={() => setPublic(prev => !prev)}
                                    label="Публикация в общий доступ"
                                />
                            </div>
                        </form>
                    </ModalContent>

                    <ModalFooter>
                        <ModalButton text="Сохранить" onClick={createCourse} />
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

export default CourseCreateModal;