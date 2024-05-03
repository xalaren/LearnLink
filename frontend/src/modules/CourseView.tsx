import ContentAbout from "../components/ContentAbout/ContentAbout";
import ContentAboutListItem from "../components/ContentAbout/ContentAboutListItem";
import ContentList from "../components/ContentList/ContentList";
import ItemLink from "../components/ItemLink";
import ProgressBar from "../components/ProgressBar";
import { Course } from "../models/course";

interface ICourseViewProps {
    course: Course;
    isCreator: boolean;
    isSubscriber: boolean;
    onSubscribe: () => void;
    onUnsubscribe: () => void;
}

function CourseView({ course, isCreator, isSubscriber, onSubscribe, onUnsubscribe }: ICourseViewProps) {
    return (
        <>
            <section className="view-page__header">
                <p className="view-page__title big-text">{course.title}</p>
            </section>

            <CourseContent
                course={course}
                onSubscribe={onSubscribe}
                onUnsubscribe={onUnsubscribe}
                isCreator={isCreator}
                isSubscriber={isSubscriber} />
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
                <ContentList
                    className="content-side__main"
                    title="Изучаемые модули"
                    showButton={props.course.localRole?.manageInternalAccess || false}
                    onHeadButtonClick={() => { }}>
                    <ItemLink
                        title="Модуль 1"
                        checked={false}
                        onClick={() => { }}
                        iconClassName="icon-module icon-medium-size"
                        className="content-list__item"
                        key={1}
                    />
                </ContentList>

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

                    <CourseProgressSection completionProgress={props.course.completionProgress} />


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

function CourseProgressSection(props: { completionProgress?: number }) {
    if (props.completionProgress == undefined) {
        return;
    }

    return (
        <>
            <ContentAboutListItem
                key={4}>
                Прогресс выполнения: <span className="text-violet">{props.completionProgress}%</span>
            </ContentAboutListItem>

            <ContentAboutListItem
                key={5}>
                <ProgressBar progress={props.completionProgress} />
            </ContentAboutListItem>
        </>
    );
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