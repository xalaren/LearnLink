import { useEffect } from "react";
import { MainContainer } from "../components/MainContainer";
import { LoginForm } from "../modules/LoginForm";
import { useAppSelector } from "../hooks/redux";
import { useHistoryNavigation } from "../hooks/historyNavigation";


export function LoginPage() {
    const isAuthenticated = useAppSelector(state => state.authReducer.isAuthenticated);
    const { toPrev } = useHistoryNavigation();

    useEffect(() => {
        if (isAuthenticated) toPrev();
    }, [isAuthenticated, toPrev]);

    return (
        <MainContainer title="Вход в систему">
            <LoginForm />
        </MainContainer>
    )
}