import { useContext, useEffect, useState } from "react";
import { Input } from "../../components/Input/Input";
import { OutlinedInput } from "../../components/Input/OutlinedInput";
import { ContentTypes, InputType, NotificationType } from "../../models/enums";
import { useUpdateLesson } from "../../hooks/lessonHook";
import { validate } from "../../helpers/validation";
import { useAppSelector } from "../../hooks/redux";
import { LessonContext } from "../../contexts/LessonContext";
import { CourseContext } from "../../contexts/CourseContext";
import PopupLoader from "../../components/Loader/PopupLoader";
import PopupNotification from "../../components/PopupNotification";
import ControlNav from "../../components/ControlNav";
import SectionCreator from "../Sections/SectionCreator";
import SectionsEditContainer from "../Sections/SectionsEditContainer";


function LessonCreator() {
    const { course } = useContext(CourseContext);
    const { lesson, fetchLesson } = useContext(LessonContext);

    const { accessToken } = useAppSelector(state => state.authReducer);
    const { user } = useAppSelector(state => state.userReducer);

    const [title, setTitle] = useState('');
    const [titleError, setTitleError] = useState('');

    const [description, setDescription] = useState('');
    const [loading, setLoading] = useState(false);

    const [creatorContentType, setCreatorContentType] = useState(ContentTypes.none);

    const updateLessonHook = useUpdateLesson();

    useEffect(() => {
        if (!lesson) return;
        setTitle(lesson.title);
        setDescription(lesson.description || '');
    }, [lesson]);

    async function procceedSave() {
        setLoading(true);
        await updateLesson();
        setLoading(false);
    }

    async function updateLesson() {
        let isValidated = true;

        if (!validate(title)) {
            setTitleError('Название урока должно быть заполнено')
            isValidated = false;
        }

        if (isValidated && user && accessToken && course && lesson) {
            const newLesson = { ...lesson, title, description };
            await updateLessonHook.updateLessonQuery(newLesson, course.id, user.id, accessToken);
        }
    }

    async function onSuccessLessonUpdate() {
        updateLessonHook.resetValues();
        await fetchLesson();
    }

    function onChange(event: React.ChangeEvent) {
        setTitleError('');

        const inputElement = event.target as HTMLInputElement;

        switch (inputElement.name) {
            case 'title':
                setTitle(inputElement.value);
                break;
            case 'description':
                setDescription(inputElement.value);
                break;
        }
    }

    return (
        <>
            {
                lesson ?
                    <div className="lesson-creator">
                        <div className="line-start-container">
                            <button className="button-violet-outline" onClick={procceedSave}>Сохранить</button>
                            <button className="button-violet-outline" onClick={fetchLesson}>Отменить</button>
                        </div>

                        <form action="#" className="lesson-creator__form">
                            <OutlinedInput
                                className="big-text"
                                name="title"
                                placeholder="Введите название урока..."
                                errorMessage={titleError}
                                required={true}
                                value={title}
                                onChange={onChange}
                            />
                            <Input
                                type={InputType.rich}
                                placeholder="Введите описание урока (необязательно)..."
                                name="description"
                                required={false}
                                onChange={onChange}
                                className="outlined-input"
                                value={description}
                            />
                        </form>


                        <section className="lesson-creator__content content-creator">
                            <div className="line-distributed-container">
                                <h3>Содержимое</h3>
                                <ControlNav>
                                    <button
                                        className="control-nav__add-button button-gray-violet icon icon-big-size icon-text-add"
                                        onClick={() => setCreatorContentType(ContentTypes.text)}
                                    ></button>
                                    <button
                                        className="control-nav__add-button button-gray-violet icon icon-big-size icon-document-add"
                                        onClick={() => setCreatorContentType(ContentTypes.file)}
                                    >
                                    </button>
                                    <button
                                        className="control-nav__add-button button-gray-violet icon icon-big-size icon-code-add"
                                        onClick={() => setCreatorContentType(ContentTypes.code)}
                                    ></button>
                                </ControlNav>
                            </div>

                            <SectionCreator
                                lessonId={lesson.id}
                                contentType={creatorContentType}
                                onClose={() => {
                                    setCreatorContentType(ContentTypes.none);
                                    fetchLesson();
                                }}
                            />
                        </section>

                        <SectionsEditContainer
                            onChange={fetchLesson}
                        />

                        {loading &&
                            <PopupLoader />
                        }

                        {updateLessonHook.error && !loading &&
                            <PopupNotification notificationType={NotificationType.error} onFade={onSuccessLessonUpdate}>
                                {updateLessonHook.error}
                            </PopupNotification>
                        }

                        {updateLessonHook.success && !loading &&
                            <PopupNotification notificationType={NotificationType.success} onFade={onSuccessLessonUpdate}>
                                {updateLessonHook.success}
                            </PopupNotification>
                        }
                    </div>
                    :
                    <p>Не удалось получить доступ к уроку...</p>
            }
        </>
    );
}

export default LessonCreator;