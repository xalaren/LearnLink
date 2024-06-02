import { useEffect, useState } from "react";
import { InputType, NotificationType } from "../../models/enums.ts";
import { Input } from "../../components/Input/Input.tsx";
import { validate } from "../../helpers/validation.ts";
import { useUpdatePassword } from "../../hooks/userHooks.ts";
import { useAppSelector } from "../../hooks/redux.ts";
import PopupNotification from "../../components/PopupNotification.tsx";

function EditProfilePasswordModule() {
    const [oldPassword, setOldPassword] = useState('');
    const [newPassword, setNewPassword] = useState('');
    const [oldPasswordError, setOldPasswordError] = useState('');
    const [newPasswordError, setNewPasswordError] = useState('');

    const { user } = useAppSelector(state => state.userReducer);
    const { accessToken } = useAppSelector(state => state.authReducer);
    const { updatePasswordQuery, error, success, resetValues } = useUpdatePassword();

    useEffect(() => {
        setOldPassword('');
        setNewPassword('');
    }, [error, success])

    async function onChange(event: React.ChangeEvent) {
        const inputElement = event.target as HTMLInputElement;

        switch (inputElement.name) {
            case 'oldPassword':
                setOldPassword(inputElement.value);
                setOldPasswordError('');
                break;
            case 'newPassword':
                setNewPassword(inputElement.value);
                setNewPasswordError('');
                break;
        }
    }

    async function onSubmit(event: React.FormEvent) {
        event.preventDefault();

        let validationError: boolean = false;

        event.preventDefault();

        if (!validate(oldPassword)) {
            setOldPasswordError("Старый пароль должен быть заполнен");
            validationError = true;
        }

        if (!validate(newPassword)) {
            setNewPasswordError("Новый пароль должен быть заполнен");
            validationError = true;
        }

        if (validationError) return;

        if (user && accessToken) {
            await updatePasswordQuery(user.id, accessToken, oldPassword, newPassword);
        }
    }

    return (
        <>
            <h3 className="account-page__title">Изменить пароль</h3>

            <form className="password-change-form" onSubmit={onSubmit}>

                <div className="form__inputs">
                    <Input
                        type={InputType.password}
                        className="form__form-input"
                        name="oldPassword"
                        errorMessage={oldPasswordError}
                        placeholder="Введите старый пароль..."
                        label="Старый пароль"
                        onChange={onChange}
                        value={oldPassword}
                        required={true}
                    />

                    <Input
                        type={InputType.password}
                        className="form__form-input"
                        name="newPassword"
                        errorMessage={newPasswordError}
                        placeholder="Введите новый пароль..."
                        label="Новый пароль"
                        onChange={onChange}
                        value={newPassword}
                        required={true}
                    />
                </div>
                <nav className="form__nav">
                    <button type="submit" className="form__button button-violet">Сохранить изменения</button>
                </nav>
            </form>

            {success &&
                <PopupNotification notificationType={NotificationType.success} onFade={resetValues}>
                    {success}
                </PopupNotification>
            }

            {error &&
                <PopupNotification notificationType={NotificationType.error} onFade={resetValues}>
                    {error}
                </PopupNotification>
            }
        </>
    );
}

export default EditProfilePasswordModule;