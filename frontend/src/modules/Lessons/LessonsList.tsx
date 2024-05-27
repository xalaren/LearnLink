import { useEffect, useState } from "react";
import ContentList from "../../components/ContentList/ContentList";
import { useGetModuleLessons } from "../../hooks/lessonHook";
import { useAppSelector } from "../../hooks/redux";
import { Course } from "../../models/course";
import { Lesson } from "../../models/lesson";
import { Module } from "../../models/module";
import { Loader } from "../../components/Loader/Loader";
import { ErrorModal } from "../../components/Modal/ErrorModal";
import { useHistoryNavigation } from "../../hooks/historyNavigation";
import LessonItem from "./LessonItem";
import LessonCreateModal from "./LessonCreateModal";

interface ILessonsListProps {
    module: Module;
    course: Course;
    updateSignal: () => void;
}

function LessonsList({ module, course, updateSignal }: ILessonsListProps) {
    const { user } = useAppSelector(state => state.userReducer);
    const { accessToken } = useAppSelector(state => state.authReducer);
    const { getLessonsAtModuleQuery, error, loading, resetValues } = useGetModuleLessons();

    const [lessons, setLessons] = useState<Lesson[]>();
    const [createModalActive, setCreateModalActive] = useState(false);

    useEffect(() => {
        fetchLessons();
    }, [user, createModalActive])

    async function fetchLessons() {
        if (!user) return;

        const foundLessons = await getLessonsAtModuleQuery(user.id, course.id, module.id, accessToken);

        if (foundLessons) {
            setLessons(foundLessons);
        }
    }

    return (
        <>

            <ContentList
                className="content-side__main"
                title="Уроки модуля"
                showButton={course.localRole?.manageInternalAccess || false}
                onHeadButtonClick={() => setCreateModalActive(true)}>
                <BuildedLessonsList
                    courseId={course.id}
                    moduleId={module.id}
                    lessons={lessons}
                    error={error}
                    loading={loading}
                    updateSignal={updateSignal}
                    onError={resetValues}
                />

            </ContentList>
            <LessonCreateModal active={createModalActive} courseId={course.id} moduleId={module.id} onClose={() => setCreateModalActive(false)} />
        </>
    );
}

function BuildedLessonsList(props: {
    error: string,
    onError: () => void,
    loading: boolean,
    courseId: number,
    moduleId: number,
    updateSignal: () => void,
    lessons?: Lesson[]
}) {
    const { toNext } = useHistoryNavigation();

    if (props.loading) {
        return (<Loader />);
    }

    if (props.error) {
        return (
            <ErrorModal active={Boolean(props.error)} onClose={props.onError} error={props.error} />
        );
    }

    return (
        <>
            {props.lessons && props.lessons.length > 0 ?
                <>{
                    props.lessons.map(lesson =>
                        <LessonItem
                            courseId={props.courseId}
                            moduleId={props.moduleId}
                            lesson={lesson}
                            updateSignal={props.updateSignal}
                        />
                    )}
                </>
                :
                <p>Нет доступных уроков</p>
            }
        </>

    );

}

export default LessonsList;