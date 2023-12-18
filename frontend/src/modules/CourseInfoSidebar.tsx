import { useEffect, useState } from "react";
import CourseSidebar from "../components/CourseSidebar";
import { useSubscription } from "../hooks/subscriptionHooks";
import { Course } from "../models/course";
import { ErrorModal } from "../components/ErrorModal";
import PopupLoader from "../ui/PopupLoader";
import { useAppSelector } from "../hooks/redux";
import { useHistoryNavigation } from "../hooks/historyNavigation";
import { Paths } from "../models/paths";

interface ICourseInfoSidebar {
    course: Course;
    isSubscriber: boolean;
    isCreator: boolean;
    subscriptionChanged: () => void;
}

function CourseInfoSidebar({ course, isSubscriber, isCreator, subscriptionChanged }: ICourseInfoSidebar) {
    const { subscribeQuery, unsubscribeQuery, error, success, loading, resetValues } = useSubscription();
    const [isErrorModalActive, setErrorModalActive] = useState(false);
    const { user } = useAppSelector(state => state.userReducer);
    const { isAuthenticated, accessToken } = useAppSelector(state => state.authReducer);
    const { toNext } = useHistoryNavigation();

    useEffect(() => {
        if (error) {
            setErrorModalActive(true);
        }
    }, [error, success, loading]);

    async function onClick(event: React.MouseEvent) {
        const target = event.target as HTMLButtonElement;

        if (!isAuthenticated) toNext(Paths.loginPath);

        switch (target.name) {
            case 'subscribeButton':
                if (user) await subscribeQuery(user.id, course.id, accessToken);
                subscriptionChanged();
                break;
            case 'unsubscribeButton':
                if (accessToken && user) await unsubscribeQuery(user.id, course.id, accessToken);
                subscriptionChanged();
                break;
        }
    }

    return (
        <>
            <CourseSidebar title="О курсе">
                <nav className={`course-sidebar__nav ${!isCreator || 'disabled'}`}>
                    {!isSubscriber && <button className="button-violet" name="subscribeButton" onClick={onClick}>Подписаться</button>}
                    {isSubscriber && <button className="button-violet" name="unsubscribeButton" onClick={onClick}>Отписаться</button>}
                </nav>
                <section className="course-sidebar__info">
                    <ul>
                        <li>
                            Видимость курса: <span className="medium-little-violet">{course.isPublic ? 'Общий доступ' : 'Приватный'}</span>
                        </li>
                        <li>
                            Подписчики: <span className="medium-little-violet">{course.subscribersCount}</span>
                        </li>
                    </ul>


                </section>
            </CourseSidebar>

            {loading && <PopupLoader />}
            <ErrorModal active={isErrorModalActive} error={error} onClose={resetValues} />
        </>
    );
}

export default CourseInfoSidebar;