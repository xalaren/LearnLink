import { useState } from "react";
import { loginAsync } from "../queries/UserQueries";
import { setAccessToken } from "../services/AccessToken";
import { ErrorModal } from "./ErrorModal";
import { LoginForm } from "./LoginForm";
import { Modal } from "./Modal";
import { useNavigate } from "react-router-dom";
import { AxiosError } from "axios";
import {useAuthorization} from "../hooks/GlobalStateHook.ts";

export function LoginPage() {
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');
    const [isModalActive, setIsModalActive] = useState(false);
    const navigate = useNavigate();
    const { refreshAuthorization } = useAuthorization();

    const openModal = () => {
        setIsModalActive(true);
    };

    const closeModal = () => {
        setIsModalActive(false);
    };

    async function fetchAuthResult(nickname: string, password: string) {
        try {
            const response = (await loginAsync(nickname, password))!;

            setSuccess(response.message);
            openModal();

            setAccessToken(response.value);
            refreshAuthorization();
        }
        catch (error: unknown) {
            const axiosError = error as AxiosError;
            setError(axiosError.message);
            openModal();
        }
    }

    return (
        <main className="main container">
            <div className="inner-container">
                <h2 className="main__title">Вход в систему: </h2>

                <LoginForm onSubmit={fetchAuthResult} />

                {error && <ErrorModal active={isModalActive} onClose={closeModal} error={error} />}

                {success &&
                    <Modal active={isModalActive} title="Успешно" onClose={() => {
                        closeModal();
                        navigate('/');
                    }}>
                        {success}
                    </Modal>
                }

            </div>
        </main>
    )
}