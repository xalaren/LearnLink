import { useEffect, useState } from "react";
import MainHeaderViaNav from "../components/ContainerHeaderViaNav";
import { Modal } from "../components/Modal";
import { Input } from "../ui/Input";
import { InputType, NotificationType, Paths, ViewTypes } from "../models/enums";
import { validate } from "../helpers/validation";
import Checkbox from "../ui/Checkbox";
import PopupLoader from "../ui/PopupLoader";
import Notification from "../ui/Notification";
import { useCreateCourse } from "../hooks/courseHooks";
import { useAppSelector } from "../hooks/redux";
import SelectionPanel from "../ui/SelectionPanel";
import { useParams } from "react-router-dom";
import { useHistoryNavigation } from "../hooks/historyNavigation";
import CreatedCourseContainer from "./CreatedCoursesContainer";
import SubscribedCoursesContainer from "./SubscribedCoursesContainer";
import { ErrorModal } from "../components/ErrorModal";


function UserCourseCreator() {
    const param = useParams<'type'>();
    const { toNext } = useHistoryNavigation();

    const [createModalActive, setCreateModalActive] = useState(false);
    const [isPublicCourse, setPublicCourse] = useState(false);

    const [courseTitle, setCourseTitle] = useState('');
    const [courseTitleError, setCourseTitleError] = useState('');

    const [courseDescription, setCourseDescription] = useState('');

    const [courseAdded, setCourseAdded] = useState(false);

    const { createCourseQuery, loading, error: createError, success, resetValues } = useCreateCourse();
    const { accessToken } = useAppSelector(state => state.authReducer);
    const { user, error: userError } = useAppSelector(state => state.userReducer);

    const [isErrorModalActive, setErrorModalActive] = useState(false);

    useEffect(() => {
        if (success) {
            handleCourseAdded();
        }
    }, [success]);

    useEffect(() => {
        if (userError) setErrorModalActive(true);
    }, [userError]);

    async function onSubmit(event: React.FormEvent) {
        event.preventDefault();

        let isValidDate = true;


        if (!validate(courseTitle)) {
            setCourseTitleError("Название курса должно быть заполнено");
            isValidDate = false;
        }
        if (!isValidDate) return;

        if (accessToken && user) {
            await createCourseQuery(courseTitle, isPublicCourse, user.id!, accessToken, courseDescription);
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

    function handleCourseAdded() {
        setCourseAdded(true);
    }

    function clearInputs() {
        setCourseTitle('');
        setCourseTitleError('');
        setCourseDescription('');
        setPublicCourse(false);
    }

    return (
        <>
            <MainHeaderViaNav title="Мои доступные курсы">
                <button className="button-gray-violet" style={{ width: '50px', height: '50px' }} onClick={() => setCreateModalActive(true)}>

                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={2.2} stroke="currentColor" style={{ width: '30px', height: '30px' }}>
                        <path strokeLinecap="round" strokeLinejoin="round" d="M12 4.5v15m7.5-7.5h-15" />
                    </svg>
                </button>
            </MainHeaderViaNav>

            <SelectionPanel selectionItems={[
                {
                    title: "Созданные",
                    onClick: () => { toNext(`${Paths.userCoursesPath}/${ViewTypes.created}`) },
                    active: param.type === ViewTypes.created
                },
                {
                    title: "Подписки",
                    onClick: () => { toNext(`${Paths.userCoursesPath}/${ViewTypes.subscribed}`) },
                    active: param.type === ViewTypes.subscribed
                }]}
            />

            {param.type === ViewTypes.created && <CreatedCourseContainer shouldUpdate={courseAdded} updateReset={() => setCourseAdded(false)} />}
            {param.type === ViewTypes.subscribed && <SubscribedCoursesContainer />}

            <Modal active={createModalActive} title="Создание курса" onClose={() => {
                setCreateModalActive(false);
                clearInputs();
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
                        <button className="button-violet" type="submit">Создать</button>
                    </nav>
                </form>
            </Modal >

            {loading && <PopupLoader />}
            {createError && <Notification notificationType={NotificationType.error} onFade={resetValues}>{createError}</Notification>}
            {success && <Notification notificationType={NotificationType.success} onFade={resetValues}>{success}</Notification>}
            <ErrorModal active={isErrorModalActive} error={userError} onClose={() => setErrorModalActive(false)} />


        </>
    );
}

export default UserCourseCreator;