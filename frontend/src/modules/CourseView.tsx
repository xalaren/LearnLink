import ContentAbout from "../components/ContentAbout/ContentAbout";
import ContentAboutListItem from "../components/ContentAbout/ContentAboutListItem";
import ProgressBar from "../components/ProgressBar";
import { Course } from "../models/course";
import ModulesList from "./ModulesList";
import EllipsisDropdown from "../components/Dropdown/EllipsisDropdown";
import DropdownItem from "../components/Dropdown/DropdownItem";
import { DropdownState } from "../contexts/DropdownContext";
import { useEffect, useState } from "react";
import { Input } from "../components/Input";
import { InputType, NotificationType } from "../models/enums";
import ModalContent from "../components/Modal/ModalContent";
import { Modal } from "../components/Modal/Modal";
import { validate } from "../helpers/validation";
import { useAppSelector } from "../hooks/redux";
import { useHideCourse, useRemoveCourse, useUpdateCourse } from "../hooks/courseHooks";
import Checkbox from "../components/Checkbox";
import ModalFooter from "../components/Modal/ModalFooter";
import ModalButton from "../components/Modal/ModalButton";
import PopupLoader from "../components/Loader/PopupLoader";
import PopupNotification from "../components/PopupNotification";
import { useHistoryNavigation } from "../hooks/historyNavigation";
import { Paths } from "../models/paths";
import { SuccessModal } from "../components/Modal/SuccessModal";

interface ICourseViewProps {
    course: Course;
    isCreator: boolean;
    isSubscriber: boolean;
    onSubscribe: () => void;
    onUnsubscribe: () => void;
    updateModalActive: boolean;
    setUpdateModalActive: (active: boolean) => void;
    deleteModalActive: boolean;
    setDeleteModalActive: (active: boolean) => void;
}

function CourseView({
    course,
    isCreator,
    isSubscriber,
    onSubscribe,
    onUnsubscribe,
    updateModalActive,
    setUpdateModalActive,
    deleteModalActive,
    setDeleteModalActive
}: ICourseViewProps) {
    const { toNext } = useHistoryNavigation();

    return (
        <>
            <section className="view-page__header">
                <p className="view-page__title big-text">{course.title}</p>
                {course.localRole?.viewAccess &&
                    <DropdownState>
                        <EllipsisDropdown>
                            {course.localRole.editAccess &&
                                <DropdownItem title="Редактировать" className="icon icon-pen-circle" key={1} onClick={() => setUpdateModalActive(true)} />
                            }

                            {course.localRole.viewAccess &&
                                <DropdownItem title="Участники" className="icon icon-user-group-circle" key={2} onClick={() => toNext(`${Paths.getCourseParticipantsPath(course.id)}/1`)} />
                            }

                            {course.localRole.removeAccess &&
                                <DropdownItem title="Удалить" className="icon icon-cross-circle" key={3} onClick={() => setDeleteModalActive(true)} />
                            }
                        </EllipsisDropdown>
                    </DropdownState>
                }

            </section>

            <CourseContent
                course={course}
                onSubscribe={onSubscribe}
                onUnsubscribe={onUnsubscribe}
                isCreator={isCreator}
                isSubscriber={isSubscriber}
            />
            <CourseEditModal active={updateModalActive} course={course} onClose={() => setUpdateModalActive(false)} />
            <CourseDeleteModal active={deleteModalActive} courseId={course.id} onClose={() => setDeleteModalActive(false)} />
        </>
    );
}

function CourseContent(props: {
    course: Course,
    isCreator: boolean,
    isSubscriber: boolean,
    onUnsubscribe: () => void,
    onSubscribe: () => void
}) {
    if (props.course.isUnavailable) {
        return (<p>Контент недоступен, так как курс скрыт</p>)
    }

    if (props.course.localRole != undefined && !props.course.localRole.viewAccess) {
        return (<p>Доступ отклонен...</p>)
    }

    return (
        <>
            <p className="view-page__description ui-text">
                {props.course.description}
            </p>

            <section className="view-page__content content-side">

                <ModulesList course={props.course} />

                <ContentAbout
                    className="content-side__aside"
                    title="О курсе">

                    <SubscribeButton
                        isCreator={props.isCreator}
                        isSubscriber={props.isSubscriber}
                        onUnsubscribe={props.onUnsubscribe}
                        onSubscribe={props.onSubscribe}
                    />

                    <ContentAboutListItem
                        key={1}>
                        Видимость курса: <span className="text-violet">
                            {getCourseVisibilityText(props.course.isPublic, props.course.isUnavailable)}
                        </span>
                    </ContentAboutListItem>

                    <ContentAboutListItem
                        key={2}>
                        Подписчики: <span className="text-violet">{props.course.subscribersCount}</span>
                    </ContentAboutListItem>

                    <ContentAboutListItem
                        key={3}>
                        Дата создания: <span className="text-violet">{props.course.creationDate}</span>
                    </ContentAboutListItem>

                    {props.course.subscribeDate != undefined &&
                        <ContentAboutListItem
                            key={4}>
                            Дата подписки: <span className="text-violet">{props.course.subscribeDate}</span>
                        </ContentAboutListItem>
                    }

                    {props.course.completionProgress != undefined &&
                        <>
                            <ContentAboutListItem
                                key={5}>
                                Прогресс выполнения: <span className="text-violet">{props.course.completionProgress}%</span>
                            </ContentAboutListItem>

                            <ContentAboutListItem
                                key={6}>
                                <ProgressBar progress={props.course.completionProgress} />
                            </ContentAboutListItem>
                        </>
                    }
                </ContentAbout>
            </section>
        </>);

}

function SubscribeButton(props: {
    isSubscriber: boolean,
    isCreator: boolean,
    onSubscribe: () => void,
    onUnsubscribe: () => void
}) {

    if (props.isCreator) {
        return;
    }

    if (props.isSubscriber) {
        return (
            <button
                className='content-about__button button-violet-outline'
                onClick={props.onUnsubscribe}>
                Отписаться
            </button>
        );
    }

    if (!props.isSubscriber) {
        return (
            <button
                className='content-about__button button-violet-outline'
                onClick={props.onSubscribe}>
                Подписаться
            </button>
        );
    }
}


function getCourseVisibilityText(isPublic: boolean, isUnavailable: boolean): string {
    if (isPublic) {
        return 'общий доступ';
    }

    if (isUnavailable) {
        return 'скрытый';
    }

    return 'приватный';
}

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

interface ICourseDeleteModalProps {
    active: boolean;
    courseId: number;
    onClose: () => void;
}

function CourseDeleteModal({ active, courseId, onClose }: ICourseDeleteModalProps) {
    const [isConfirmed, setConfirmed] = useState(false);
    const [isSubmitted, setSubmitted] = useState(false);
    const [error, setError] = useState('');

    const { user } = useAppSelector(state => state.userReducer);
    const { accessToken } = useAppSelector(state => state.authReducer);
    const { toNext } = useHistoryNavigation();

    const removeCourseHook = useRemoveCourse();
    const hideCourseHook = useHideCourse();



    function closeModal() {
        resetDefault();
        onClose();
    }

    function resetDefault() {
        setConfirmed(false);
        setSubmitted(false);
        removeCourseHook.resetValues();
        hideCourseHook.resetValues();
    }

    async function onDeleteSubmit() {
        setSubmitted(true);

        if (isConfirmed) {
            if (user && accessToken) {
                await removeCourseHook.removeCourseQuery(courseId, user.id, accessToken);
            }

            if (removeCourseHook.error) {
                setError(removeCourseHook.error);
                return;
            }

        }
    }

    async function onHideSubmit() {
        if (user && accessToken) {
            await hideCourseHook.hideCourseQuery(courseId, user.id, accessToken);
        }

        if (hideCourseHook.error) {
            setError(hideCourseHook.error);
            return;
        }
    }

    return (
        <>
            <Modal
                active={active && !removeCourseHook.success}
                title="Подтвердите действие"
                onClose={onClose}>

                <ModalContent className="indented">
                    {isSubmitted && !isConfirmed &&
                        <p className="error-text required">Вы не подтвердили действие</p>
                    }
                    <Checkbox
                        checkedChanger={() => { setConfirmed(prev => !prev) }}
                        isChecked={isConfirmed}
                        labelClassName="ui-text"
                        label="Вы подтверждаете удаление курсов? Это повлечет удаление всех модулей, уроков, а также прогрессов, сделанных участниками этого курса.Рекомендуем скрыть курс вместо полного удаления!" />
                </ModalContent>

                <ModalFooter>
                    <ModalButton text="Скрыть курс" onClick={onHideSubmit} />
                    <ModalButton className="button-danger-light" text="Удалить курс" onClick={onDeleteSubmit} />
                </ModalFooter>

            </Modal>

            {removeCourseHook.loading || hideCourseHook.loading &&
                <PopupLoader />
            }

            {error &&
                <PopupNotification notificationType={NotificationType.error} onFade={closeModal}>
                    {error}
                </PopupNotification>
            }

            {removeCourseHook.success &&
                <SuccessModal active={Boolean(removeCourseHook.success)} message="Курс успешно удален" onClose={() => toNext(Paths.userCoursesPath + '/created')}>
                </SuccessModal>
            }

            {hideCourseHook.success &&
                <PopupNotification notificationType={NotificationType.success} onFade={resetDefault}>
                    {hideCourseHook.success}
                </PopupNotification>
            }
        </>
    );
}

export default CourseView;