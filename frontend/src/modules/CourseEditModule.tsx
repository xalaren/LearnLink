import { useEffect, useState } from "react";
import { Modal } from "../components/Modal";
import { validate } from "../helpers/validation";
import { Input } from "../components/Input";
import { InputType, NotificationType } from "../models/enums";
import Checkbox from "../components/Checkbox";
import { Course } from "../models/course";
import { useUpdateCourse } from "../hooks/courseHooks";
import PopupLoader from "../components/PopupLoader";
import Notification from "../components/PopupNotification";
import { useAppSelector } from "../hooks/redux";

interface ICourseEditModuleProps {
    active: boolean;
    defaultCourse: Course;
    onClose: () => void;
    refreshRequest: () => void;
}

function CourseEditModule({ active, defaultCourse, onClose, refreshRequest }: ICourseEditModuleProps) {
    const { updateCourseQuery, loading, error, success, resetValues } = useUpdateCourse();

    const [isPublicCourse, setPublicCourse] = useState(false);

    const [courseTitle, setCourseTitle] = useState('');
    const [courseTitleError, setCourseTitleError] = useState('');

    const [courseDescription, setCourseDescription] = useState('');

    const { user } = useAppSelector(state => state.userReducer);
    const { accessToken } = useAppSelector(state => state.authReducer);

    useEffect(() => {
        resetInputs();
    }, [defaultCourse.description, defaultCourse.isPublic, defaultCourse.title]);


    function resetInputs() {
        setCourseTitle(defaultCourse.title);
        setCourseTitleError('');
        setCourseDescription(defaultCourse.description || '');
        setPublicCourse(defaultCourse.isPublic);
    }

    async function onSubmit(event: React.FormEvent) {
        event.preventDefault();

        let isValidDate = true;


        if (!validate(courseTitle)) {
            setCourseTitleError("Название курса должно быть заполнено");
            isValidDate = false;
        }
        if (!isValidDate) return;

        if (accessToken && user) {
            await updateCourseQuery(courseTitle, isPublicCourse, user.id, defaultCourse.id, accessToken, courseDescription);
            refreshRequest();
        }

    }

    function onChange(event: React.ChangeEvent) {
        const inputElement = event.target as HTMLInputElement;

        switch (inputElement.name) {
            case 'courseTitle':
                setCourseTitle(inputElement.value);
                setCourseTitleError('');
                break;
            case 'courseDescription':
                setCourseDescription(inputElement.value);
                break;
        }
    }

    return (
        <>
            <Modal active={active} title="Редактирование курса" onClose={() => {
                resetInputs();
                onClose();
            }}>
                <form className="base-form" onSubmit={onSubmit}>
                    <ul className="form__inputs">
                        <p className="medium-little">
                            Название курса
                        </p>
                        <Input
                            type={InputType.text}
                            name="courseTitle"
                            onChange={onChange}
                            errorMessage={courseTitleError}
                            placeholder="Введите название курса..."
                            width={500}
                            value={courseTitle}
                        />

                        <p className="medium-little">Описание курса</p>
                        <Input
                            type={InputType.rich}
                            name="courseDescription"
                            onChange={onChange}
                            placeholder="Введите описание курса (Необязательно)..."
                            styles={{ width: '500px' }}
                            value={courseDescription}
                        />
                        <Checkbox label="Общедоступный курс" isChecked={isPublicCourse} checkedChanger={() => { setPublicCourse(prev => !prev) }} />
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

export default CourseEditModule;