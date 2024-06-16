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

interface AnswersListProps {
    courseId: number,
    moduleId: number,
    lessonId: number,
    objectiveId: number
}

function AnswersList({ courseId, moduleId, lessonId, objectiveId }: AnswersListProps) {
    const [answers, setAnswers] = useState<Answer[]>();
    const [page, setPage] = useState(1);
    const [pageCount, setPageCount] = useState(1);

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
            const dataPage = await getAnswersFromObjectiveQuery(user.id, courseId, lessonId, objectiveId, page, 4, accessToken)

            if (dataPage) {
                setAnswers(dataPage.values);
                setPageCount(dataPage.pageCount);
            }
        }
    }

    return (
        <>
            <div className="line-distributed-container">
                <h3>Ответы участников ({answers?.length || 0}): </h3>

                <ControlNav>
                    <button
                        className="control-nav__add-button button-gray icon icon-medium-size icon-plus">
                    </button>
                </ControlNav>
            </div>

            <Paginate currentPage={page} pageCount={pageCount} setPage={setPage} />

            <BuildedAnswersList
                courseId={courseId}
                moduleId={moduleId}
                lessonId={lessonId}
                objectiveId={objectiveId}
                error={error}
                onError={resetValues}
                loading={loading}
                answers={answers}
            />
        </>
    );
}

function BuildedAnswersList(props: {
    courseId: number,
    moduleId: number,
    lessonId: number,
    objectiveId: number,
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
                            onReviewClick={() => { }}
                            onViewClick={() => { }}
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