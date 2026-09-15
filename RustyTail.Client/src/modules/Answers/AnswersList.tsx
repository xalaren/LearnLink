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
import AnswerCreateModal from "./AnswerCreateModal";
import CreateReviewModal from "../Reviews/CreateReviewModal";

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

    const [createModalActive, setCreateModalActive] = useState(false);
    const [selectedAnswerId, setSelectedAnswerId] = useState(0);

    const { user } = useAppSelector(state => state.userReducer);
    const { accessToken } = useAppSelector(state => state.authReducer);

    const { getAnswersFromObjectiveQuery, error, loading, resetValues } = useAnswerQueries();

    const { toNext } = useHistoryNavigation();

    useEffect(() => {
        if (!user || !accessToken || createModalActive || selectedAnswerId) return;

        fetchAnswers();
    }, [user, accessToken, createModalActive, selectedAnswerId])

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
                                className="control-nav__add-button button-gray icon icon-medium-size icon-plus"
                                onClick={() => setCreateModalActive(true)}
                            >
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
                        loading={loading}>

                        {answers && answers.length > 0 ?
                            answers.map(answer =>
                                <AnswerItem
                                    key={answer.id}
                                    answer={answer}
                                    showReviewButton={course.localRole!.manageInternalAccess && !answer.grade}
                                    showViewButton={user.id === answer.userDetails.id || course.localRole!.manageInternalAccess}
                                    onReviewClick={() => {
                                        setSelectedAnswerId(answer.id)
                                    }}
                                    onViewClick={() => {
                                        toNext(paths.answer.view.full(
                                            course.id,
                                            moduleId,
                                            lessonId,
                                            objectiveId,
                                            answer.id))
                                    }}
                                />
                            ) :
                            <p className="optional-text">Ответы пока не загружены...</p>
                        }
                    </BuildedAnswersList>

                    <AnswerCreateModal
                        lessonId={lessonId}
                        objectiveId={objectiveId}
                        active={Boolean(createModalActive)}
                        onClose={() => setCreateModalActive(false)}
                    />

                    <CreateReviewModal
                        active={selectedAnswerId > 0}
                        answerId={selectedAnswerId}
                        onClose={() => setSelectedAnswerId(0)}
                    />
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
    children: React.ReactNode
}) {

    if (props.loading) {
        return (<Loader />);
    }

    if (props.error) {
        return (
            <ErrorModal active={Boolean(props.error)} onClose={props.onError} error={props.error} />
        );
    }

    return (props.children);

}

export default AnswersList;