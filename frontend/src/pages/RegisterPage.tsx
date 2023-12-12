import { useEffect } from "react";
import { MainContainer } from "../components/MainContainer";
import { useAppSelector } from "../hooks/redux";
import { RegisterForm } from "../modules/RegisterForm";
import { useHistoryNavigation } from "../hooks/historyNavigation";
import { Paths } from "../models/enums";


export function RegisterPage() {
    const isAuthenticated = useAppSelector(state => state.authReducer.isAuthenticated);
    const { toNext } = useHistoryNavigation();

    useEffect(() => {
        if (isAuthenticated) toNext(Paths.homePath);
    }, [isAuthenticated, toNext]);

    return (
        <MainContainer title="Регистрация">
            <RegisterForm />
        </MainContainer>
    )
}