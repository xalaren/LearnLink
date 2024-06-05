import { createContext, useEffect, useState } from "react";
import { Module } from "../models/module";
import React from "react";
import { useAppSelector } from "../hooks/redux";
import { useParams } from "react-router-dom";
import { useGetModule } from "../hooks/moduleHooks";
import { MainContainer } from "../components/MainContainer";
import { ErrorModal } from "../components/Modal/ErrorModal";
import { Loader } from "../components/Loader/Loader";
import { Lesson } from "../models/lesson";
import { useGetLesson } from "../hooks/lessonHook";


interface ILessonContext {
    lesson: Lesson | null;
    fetchLesson: () => Promise<void>;
}

export const LessonContext = createContext<ILessonContext>({
    lesson: null,
    fetchLesson: async () => { },
})

export const LessonState = ({ children }: { children: React.ReactNode }) => {
    const [lesson, setLesson] = useState<Lesson | null>(null);


    const { user } = useAppSelector(state => state.userReducer);
    const { accessToken } = useAppSelector(state => state.authReducer);
    const { getLessonQuery, error, loading, resetValues } = useGetLesson();

    const courseParam = useParams<'courseId'>();
    const lessonParam = useParams<'lessonId'>();

    useEffect(() => {
        if (!lessonParam || !courseParam) return;

        if (!lesson) {
            fetchLesson();
        }
    }, [user]);

    async function fetchLesson() {
        if (!user) {
            return;
        }
        const result = await getLessonQuery(user.id, Number(courseParam.courseId), Number(lessonParam.lessonId), accessToken);

        if (result) {
            setLesson(result);
        }
    }

    return (
        <LessonContext.Provider value={{ lesson, fetchLesson }}>
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

        </LessonContext.Provider>
    )
}