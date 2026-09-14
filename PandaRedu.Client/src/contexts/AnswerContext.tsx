import { createContext, useEffect, useState } from "react";
import React from "react";
import { useAppSelector } from "../hooks/redux";
import { useParams } from "react-router-dom";
import { MainContainer } from "../components/MainContainer";
import { ErrorModal } from "../components/Modal/ErrorModal";
import { Loader } from "../components/Loader/Loader";
import { Answer } from "../models/answer";
import { useAnswerQueries } from "../hooks/answerHooks";


interface IAnswerContext {
    answer: Answer | null;
    fetchAnswer: () => Promise<void>;
}

export const AnswerContext = createContext<IAnswerContext>({
    answer: null,
    fetchAnswer: async () => { },
})

export const AnswerState = ({ children }: { children: React.ReactNode }) => {
    const [answer, setAnswer] = useState<Answer | null>(null);


    const { user } = useAppSelector(state => state.userReducer);
    const { accessToken } = useAppSelector(state => state.authReducer);
    const { getAnswerQuery, error, loading, resetValues } = useAnswerQueries();

    const courseParam = useParams<'courseId'>();
    const lessonParam = useParams<'lessonId'>();
    const answerParam = useParams<'answerId'>();

    useEffect(() => {
        if (!lessonParam || !answerParam || !user) return;

        if (!answer) {
            fetchAnswer();
        }
    }, [user]);

    async function fetchAnswer() {
        const result = await getAnswerQuery(
            user!.id,
            Number(courseParam.courseId),
            Number(lessonParam.lessonId),
            Number(answerParam.answerId),
            accessToken);

        if (result) {
            setAnswer(result);
        }
    }

    return (
        <AnswerContext.Provider value={{ answer, fetchAnswer }}>
            {error &&
                <MainContainer>
                    <ErrorModal active={Boolean(error)} error={error} onClose={resetValues}>

                    </ErrorModal>
                </MainContainer>
            }
            {!error && loading &&
                <MainContainer>
                    <Loader />
                </MainContainer>
            }

            {!error && !loading &&
                <>
                    {children}
                </>
            }

        </AnswerContext.Provider>
    )
}