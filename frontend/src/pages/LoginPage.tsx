import { useState } from "react";
import { getUserAsync, loginAsync } from "../queries/UserQueries.ts";
import { ErrorModal } from "../components/ErrorModal.tsx";
import { LoginForm } from "../components/LoginForm.tsx";
import { Modal } from "../components/Modal.tsx";
import { useNavigate } from "react-router-dom";
import { AxiosError } from "axios";
import { AppDispatch } from "../store/index.ts";
import { AuthorizationSlice } from "../store/slices/AuthorizationSlice.ts";

export function LoginPage() {
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');
    const [isModalActive, setIsModalActive] = useState(false);
    const navigate = useNavigate();

    const openModal = () => {
        setIsModalActive(true);
    };

    const closeModal = () => {
        setIsModalActive(false);
    };

    function fetchAuthResult(nickname: string, password: string) {
        return async (dispatch: AppDispatch) => {
            try {
                const response = (await loginAsync(nickname, password))!;

                const user = (await getUserAsync(response.value))!.value;

                setSuccess(response.message);
                openModal();

                dispatch(AuthorizationSlice.actions.onLogin({
                    accessToken: response.value,
                    user: user,
                }));
            }
            catch (error: unknown) {
                const axiosError = error as AxiosError;
                setError(axiosError.message);
                openModal();
            }
        }
    }

    return (
        <main className="main container">
            <div className="inner-container">
                <h2 className="main__title">Вход в систему</h2>

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