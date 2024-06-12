import { createContext, useEffect, useState } from "react";
import React from "react";
import { useAppSelector } from "../hooks/redux";
import { useParams } from "react-router-dom";
import { MainContainer } from "../components/MainContainer";
import { ErrorModal } from "../components/Modal/ErrorModal";
import { Loader } from "../components/Loader/Loader";
import { Objective } from "../models/objective";
import { useObjectiveQueries } from "../hooks/objectiveHooks";


interface IObjectiveContext {
    objective: Objective | null;
    fetchObjective: () => Promise<void>;
}

export const ObjectiveContext = createContext<IObjectiveContext>({
    objective: null,
    fetchObjective: async () => { },
})

export const ObjectiveState = ({ children }: { children: React.ReactNode }) => {
    const [objective, setObjective] = useState<Objective | null>(null);


    const { user } = useAppSelector(state => state.userReducer);
    const { accessToken } = useAppSelector(state => state.authReducer);
    const { getObjectiveQuery, error, loading, resetValues } = useObjectiveQueries();

    const lessonParam = useParams<'lessonId'>();
    const objectiveParam = useParams<'objectiveId'>();

    useEffect(() => {
        if (!lessonParam || !objectiveParam || !user) return;

        if (!objective) {
            fetchObjective();
        }
    }, [user]);

    async function fetchObjective() {
        const result = await getObjectiveQuery(Number(lessonParam.lessonId), Number(objectiveParam.objectiveId), accessToken);

        if (result) {
            setObjective(result);
        }
    }

    return (
        <ObjectiveContext.Provider value={{ objective, fetchObjective }}>
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

        </ObjectiveContext.Provider>
    )
}