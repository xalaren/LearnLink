import { useEffect } from "react";
import { MainContainer } from "../components/MainContainer";
import { useNavigate } from "react-router-dom";
import { LoginForm } from "../modules/LoginForm";
import { useAppSelector } from "../hooks/redux";


export function LoginPage() {
    const isAuthenticated = useAppSelector(state => state.authReducer.isAuthenticated);
    const navigate = useNavigate();

    useEffect(() => {
        if (isAuthenticated) navigate('/');
    }, [isAuthenticated, navigate]);

    return (
        <MainContainer title="Вход в систему">
            <LoginForm />

        </MainContainer>
    )
}