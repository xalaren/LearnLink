import { useState } from "react";
import { EditUserForm } from "../components/EditUserForm.tsx";
import { User } from "../models/User.ts";
import { updateUserAsync, updateUserPasswordAsync } from "../queries/UserQueries.ts";
import { AxiosError } from "axios";
import { ErrorModal } from "../components/ErrorModal.tsx";
import { validate } from "../services/Validation.ts";
import { Modal } from "../components/Modal.tsx";
import { getAccessToken, setAccessToken } from "../services/AccessToken.ts";
import { EditUserPassForm } from "../components/EditUserPassForm.tsx";
import { EditActions } from "../models/Constants.ts";

interface IEditUserPageProps {
    action: EditActions;
}

export function EditUserPage({ action }: IEditUserPageProps) {
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    async function fetchUpdateUserResult(user: User) {
        try {
            const token = getAccessToken();

            if (!token) {
                return;
            }

            if (!user) {
                return;
            }

            const response = (await updateUserAsync(user, token))!;

            if (!response.success) {
                setError(response.message);
                return;
            }

            setSuccess(response.message);
            setAccessToken(response.value);

        }
        catch (error: unknown) {
            const axiosError = error as AxiosError;
            setError(axiosError.message);
        }
    }

    async function fetchUpdateUserPasswordResult(oldPassword: string, newPassword: string) {
        try {
            const token = getAccessToken();

            if (!token) {
                return;
            }

            if (!user) {
                return;
            }

            const response = (await updateUserPasswordAsync(user.id, token, oldPassword, newPassword))!;

            if (!response.success) {
                setError(response.message);
                return;
            }
            setSuccess(response.message);
        }
        catch (error: unknown) {
            const axiosError = error as AxiosError;
            setError(axiosError.message);
        }

    }

    return (
        <main className="main container">
            <div className="inner-container">
                <h2 className="main__title">Редактировать профиль </h2>

                {user && action == EditActions.EditUser &&
                    <EditUserForm defaultValues={user} onSubmit={fetchUpdateUserResult} />
                }

                {user && action == EditActions.EditPassword &&
                    <EditUserPassForm onSubmit={fetchUpdateUserPasswordResult} />
                }

                {error &&
                    <ErrorModal error={error} active={validate(error)} onClose={() => setError('')}></ErrorModal>
                }

                {success &&
                    <Modal title="Успешно" active={validate(success)} onClose={() => setSuccess('')}>
                        {success}
                    </Modal>
                }
            </div>
        </main>
    );
}