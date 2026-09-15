import { createContext, useEffect, useState } from "react";
import { Module } from "../models/module";
import React from "react";
import { useAppSelector } from "../hooks/redux";
import { useParams } from "react-router-dom";
import { useGetModule } from "../hooks/moduleHooks";
import { MainContainer } from "../components/MainContainer";
import { ErrorModal } from "../components/Modal/ErrorModal";
import { Loader } from "../components/Loader/Loader";


interface IModuleContext {
    module: Module | null;
    fetchModule: () => Promise<void>;
}

export const ModuleContext = createContext<IModuleContext>({
    module: null,
    fetchModule: async () => { },
})

export const ModuleState = ({ children }: { children: React.ReactNode }) => {
    const [module, setModule] = useState<Module | null>(null);


    const { user } = useAppSelector(state => state.userReducer);
    const { accessToken } = useAppSelector(state => state.authReducer);
    const { getModuleQuery, loading, error, resetValues } = useGetModule();

    const courseParam = useParams<'courseId'>();
    const moduleParam = useParams<'moduleId'>();

    useEffect(() => {
        if (!moduleParam || !courseParam) return;

        if (!module) {
            fetchModule();
        }
    }, [user]);

    async function fetchModule() {
        if (!user) {
            return;
        }
        const result = await getModuleQuery(user.id, Number(courseParam.courseId), Number(moduleParam.moduleId), accessToken);

        if (result) {
            setModule(result);
        }
    }

    return (
        <ModuleContext.Provider value={{ module, fetchModule }}>
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

        </ModuleContext.Provider>
    )
}