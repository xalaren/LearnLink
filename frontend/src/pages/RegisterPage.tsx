import { useEffect } from "react";
import { MainContainer } from "../components/MainContainer";
import { useNavigate } from "react-router-dom";
import { useAppSelector } from "../hooks/redux";
import { RegisterForm } from "../modules/RegisterForm";


export function RegisterPage() {
    const isAuthenticated = useAppSelector(state => state.authReducer.isAuthenticated);
    const navigate = useNavigate();

    useEffect(() => {
        if (isAuthenticated) navigate('/');
    }, [isAuthenticated, navigate]);

    return (
        <MainContainer title="Регистрация">
            <RegisterForm />
        </MainContainer>
    )
}