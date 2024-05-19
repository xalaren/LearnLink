import ContentAbout from "../../components/ContentAbout/ContentAbout";
import ContentAboutListItem from "../../components/ContentAbout/ContentAboutListItem";
import ProgressBar from "../../components/ProgressBar";
import { Course } from "../../models/course";
import ModulesList from "../Modules/ModulesList";
import EllipsisDropdown from "../../components/Dropdown/EllipsisDropdown";
import DropdownItem from "../../components/Dropdown/DropdownItem";
import { DropdownState } from "../../contexts/DropdownContext";
import { useHistoryNavigation } from "../../hooks/historyNavigation";
import CourseEditModal from "./CourseEditModal";
import CourseDeleteModal from "./CourseDeleteModal";
import { paths } from "../../models/paths";
import { useNavigate } from "react-router-dom";

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
                                <DropdownItem title="Участники" className="icon icon-user-group-circle" key={2} onClick={() => toNext(paths.course.participants.full(course.id), true)} />
                            }

                            {course.localRole.editRolesAccess &&
                                <DropdownItem title="Роли" className="icon icon-star" key={2} onClick={() => toNext(paths.course.roles.full(course.id), true)} />
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
            <CourseEditModal active={updateModalActive} onClose={() => setUpdateModalActive(false)} />
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

export default CourseView;