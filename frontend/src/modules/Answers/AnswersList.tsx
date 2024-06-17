import { useEffect, useState } from "react";
import { Loader } from "../../components/Loader/Loader";
import { ErrorModal } from "../../components/Modal/ErrorModal";
import AnswerItem from "../../components/UsersList/AnswerItem";
import { useHistoryNavigation } from "../../hooks/historyNavigation";
import { Answer } from "../../models/answer";
import Paginate from "../../components/Paginate";
import ControlNav from "../../components/ControlNav";
import { useAnswerQueries } from "../../hooks/answerHooks";
import { useAppSelector } from "../../hooks/redux";
import { Course } from "../../models/course";
import { LocalRole } from "../../models/localRole";
import { paths } from "../../models/paths";

interface AnswersListProps {
    course: Course,
    moduleId: number,
    lessonId: number,
    objectiveId: number
}

function AnswersList({ course, moduleId, lessonId, objectiveId }: AnswersListProps) {
    const [answers, setAnswers] = useState<Answer[]>();
    const [page, setPage] = useState(1);
    const [pageCount, setPageCount] = useState(1);
    const [itemsCount, setItemsCount] = useState(0);

    const { user } = useAppSelector(state => state.userReducer);
    const { accessToken } = useAppSelector(state => state.authReducer);

    const { getAnswersFromObjectiveQuery, error, loading, resetValues } = useAnswerQueries();

    useEffect(() => {
        if (!user || !accessToken) return;

        fetchAnswers();
    }, [user, accessToken])

    async function fetchAnswers() {
        resetValues();

        if (user && accessToken) {
            const dataPage = await getAnswersFromObjectiveQuery(user.id, course.id, lessonId, objectiveId, page, 4, accessToken)

            if (dataPage) {
                setAnswers(dataPage.values);
                setPageCount(dataPage.pageCount);
                setItemsCount(dataPage.itemsCount);
            }
        }
    }

    return (
        <>
            {user && course && course.localRole ?
                <>
                    <div className="line-distributed-container">
                        <h3>Ответы участников ({itemsCount}): </h3>

                        <ControlNav>
                            <button
                                className="control-nav__add-button button-gray icon icon-medium-size icon-plus">
                            </button>
                        </ControlNav>
                    </div>

                    <Paginate currentPage={page} pageCount={pageCount} setPage={setPage} />

                    <BuildedAnswersList
                        localRole={course.localRole}
                        courseId={course.id}
                        moduleId={moduleId}
                        lessonId={lessonId}
                        objectiveId={objectiveId}
                        userId={user.id}
                        error={error}
                        onError={resetValues}
                        loading={loading}
                        answers={answers} />
                </> :
                <p>Ошибка доступа к ответам...</p>
            }
        </>
    );
}

function BuildedAnswersList(props: {
    localRole: LocalRole,
    courseId: number,
    moduleId: number,
    lessonId: number,
    objectiveId: number,
    userId: number,
    error: string,
    onError: () => void,
    loading: boolean,
    answers?: Answer[]
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
            {props.answers && props.answers.length > 0 ?
                <>{
                    props.answers.map(answer =>
                        <AnswerItem
                            key={answer.id}
                            answer={answer}
                            showReviewButton={props.localRole.manageInternalAccess}
                            showViewButton={props.userId === answer.userDetails.id || props.localRole.manageInternalAccess}
                            onReviewClick={() => {

                            }}
                            onViewClick={() => {
                                toNext(paths.answer.view.full(
                                    props.courseId,
                                    props.moduleId,
                                    props.lessonId,
                                    props.objectiveId,
                                    answer.id))
                            }}
                        />
                    )}
                </>
                :
                <p>Ответы пока не загружены...</p>
            }
        </>

    );

}

export default AnswersList;