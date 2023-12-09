import { useState } from "react";
import { User } from "../models/User";
import { registerAsync } from "../queries/UserQueries";
import { RegisterForm } from "../components/RegisterForm";
import { AxiosError } from "axios";
import { ErrorModal } from "../components/ErrorModal";
import { useNavigate } from "react-router-dom";
import { Modal } from "../components/Modal";

export function RegisterPage() {

    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');
    const [isModalActive, setIsModalActive] = useState(false);
    const navigate = useNavigate();

    async function fetchRegisterResult(user: User, password: string) {
        try {
            const response = (await registerAsync(user, password))!;

            setSuccess(response.message);
        }
        catch (error: unknown) {
            const axiosError = error as AxiosError;
            setError(axiosError.message);
            openModal();
        }
    }

    const openModal = () => {
        setIsModalActive(true);
    };

    const closeModal = () => {
        setIsModalActive(false);
    };

    return (
        <main className="main container">
            <div className="inner-container">
                <h2 className="main__title">Регистрация нового пользователя </h2>

                <RegisterForm onSubmit={fetchRegisterResult} />

                {error &&
                    <ErrorModal active={isModalActive} onClose={closeModal} error={error} />
                }

                {success &&
                    <Modal active={isModalActive} title="Успешно" onClose={() => {
                        closeModal();
                        navigate('/login');
                    }}>
                        <>Регистрация прошла успешно. Теперь можно войти</>
                    </Modal>
                }
            </div>
        </main >
    )
}