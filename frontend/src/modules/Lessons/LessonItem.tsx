import ControlItemLink from "../../components/ItemLink/ControlItemLink";
import PopupNotification from "../../components/PopupNotification";
import { useCompleteLesson } from "../../hooks/completionHook";
import { useAppSelector } from "../../hooks/redux";
import { NotificationType } from "../../models/enums";
import { Lesson } from "../../models/lesson";

interface ILessonItemProps {
    lesson: Lesson;
    courseId: number;
    moduleId: number;
    onClick: () => void;
    updateSignal: () => void;
}

function LessonItem({ lesson, courseId, moduleId, onClick, updateSignal }: ILessonItemProps) {
    const { completeLessonQuery, error, success, loading, resetValues } = useCompleteLesson();
    const { user } = useAppSelector(state => state.userReducer);
    const { accessToken } = useAppSelector(state => state.authReducer);

    async function completeLesson() {
        if (!user || !accessToken) return;
        await completeLessonQuery(true, user.id, courseId, moduleId, lesson.id, accessToken);
    }

    async function uncompleteLesson() {
        if (!user || !accessToken) return;
        await completeLessonQuery(false, user.id, courseId, moduleId, lesson.id, accessToken);
    }

    function onFade() {
        resetValues();
        updateSignal();
    }

    return (
        <>
            <ControlItemLink
                title={lesson.title}
                checked={lesson.completed}
                onClick={onClick}
                className="content-list__item"
                key={lesson.id}
                loading={loading || Boolean(success) || Boolean(error)}
                onCheck={completeLesson}
                onUncheck={uncompleteLesson}>
                <span className="icon icon-medium-size icon-badge"></span>
            </ControlItemLink>
            {error &&
                <PopupNotification notificationType={NotificationType.error} onFade={onFade}>
                    {error}
                </PopupNotification>
            }

            {success &&
                <PopupNotification notificationType={NotificationType.success} onFade={onFade}>
                    {success}
                </PopupNotification>
            }
        </>

    );
}

export default LessonItem;