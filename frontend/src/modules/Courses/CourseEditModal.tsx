import { useEffect, useState } from "react";
import { Course } from "../../models/course";
import { useAppSelector } from "../../hooks/redux";
import { useUpdateCourse } from "../../hooks/courseHooks";
import { validate } from "../../helpers/validation";
import { Modal } from "../../components/Modal/Modal";
import ModalContent from "../../components/Modal/ModalContent";
import { Input } from "../../components/Input";
import { InputType, NotificationType } from "../../models/enums";
import Checkbox from "../../components/Checkbox";
import ModalFooter from "../../components/Modal/ModalFooter";
import ModalButton from "../../components/Modal/ModalButton";
import PopupLoader from "../../components/Loader/PopupLoader";
import PopupNotification from "../../components/PopupNotification";

interface ICourseUpdateModalProps {
    active: boolean;
    course: Course;
    onClose: () => void;
}

function CourseEditModal({ active, course, onClose }: ICourseUpdateModalProps) {
    const [title, setTitle] = useState(course.title);
    const [description, setDescription] = useState(course.description);
    const [isPublic, setPublic] = useState(course.isPublic);
    const [isUnavailable, setUnavailable] = useState(course.isUnavailable);
    const [titleError, setTitleError] = useState('');

    const { accessToken } = useAppSelector(state => state.authReducer);
    const { user } = useAppSelector(state => state.userReducer);
    const { updateCourseQuery, loading, error, success, resetValues } = useUpdateCourse();

    useEffect(() => {
    }, [user])

    function closeModal() {
        resetValues();
        setTitle(course.title);
        setDescription(course.description);
        setTitleError('');
        setPublic(course.isPublic);
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

    async function updateCourse() {
        let isValidated = true;

        if (!validate(title)) {
            setTitleError('Название курса должно быть заполнено')
            isValidated = false;
        }

        if (isValidated && user && accessToken) {
            const newCourse = new Course(course.id, title, isPublic, isUnavailable, description);
            await updateCourseQuery(newCourse, user.id, accessToken);
        }
    }

    return (
        <>
            <Modal
                active={active}
                onClose={closeModal}
                title="Редактирование курса">

                <ModalContent>
                    <form className="form">
                        <div className="form__inputs">
                            <Input
                                type={InputType.text}
                                name="title"
                                label="Название курса"
                                placeholder="Введите название..."
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
                                checkedChanger={() => {
                                    setUnavailable(false);
                                    setPublic(prev => !prev);
                                }}
                                label="Публикация в общий доступ"
                            />

                            <Checkbox
                                isChecked={isUnavailable}
                                checkedChanger={() => {
                                    setPublic(false);
                                    setUnavailable(prev => !prev);
                                }}
                                label="Скрыть курс"
                            />
                        </div>
                    </form>
                </ModalContent>

                <ModalFooter>
                    <ModalButton text="Сохранить" onClick={updateCourse} />
                </ModalFooter>

            </Modal >

            {loading &&
                <PopupLoader />
            }

            {success &&
                <PopupNotification notificationType={NotificationType.success} onFade={resetValues}>
                    {success}
                </PopupNotification>
            }

            {error &&
                <PopupNotification notificationType={NotificationType.error} onFade={resetValues}>
                    {error}
                </PopupNotification>
            }
        </>
    );

}

export default CourseEditModal;