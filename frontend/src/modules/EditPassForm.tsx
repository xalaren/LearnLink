import { useEffect, useState } from "react";
import { validate } from "../helpers/validation";
import { InputType } from "../helpers/enums";
import { Input } from "../ui/Input";
import { useAppDispatch, useAppSelector } from "../hooks/redux";
import { useUpdatePassword } from "../hooks/userHooks";
import { ErrorModal } from "../components/ErrorModal";
import { SuccessModal } from "../components/SuccessModal";

function EditPassForm() {
    const { accessToken } = useAppSelector(state => state.authReducer);
    const { user } = useAppSelector(state => state.userReducer);
    const { updatePasswordQuery, error, success, resetValues } = useUpdatePassword();
    const dispatch = useAppDispatch();

    const [isErrorModalActive, setErrorModalActive] = useState(false);
    const [isSuccessModalActive, setSuccessModalActive] = useState(false);

    const [oldPassword, setOldPassword] = useState('');
    const [newPassword, setNewPassword] = useState('');
    const [oldPasswordError, setOldPasswordError] = useState('');
    const [newPasswordError, setNewPasswordError] = useState('');

    useEffect(() => {
        if (error) {
            setErrorModalActive(true);
        }

        if (success) {
            setSuccessModalActive(true);
        }
    }, [dispatch, error, success]);

    const onSubmit = async (event: React.FormEvent) => {
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

        if (user) await updatePasswordQuery(user.id!, accessToken, oldPassword, newPassword);
    }

    function onChange(event: React.ChangeEvent) {
        const inputElement = event.target as HTMLInputElement;

        switch (inputElement.name) {
            case 'old-pass':
                setOldPassword(inputElement.value);
                setOldPasswordError('');
                break;
            case 'new-pass':
                setNewPassword(inputElement.value);
                setNewPasswordError('');
                break;
        }
    }

    function closeErrorModal() {
        setErrorModalActive(false);
        resetValues();
    }

    function closeSuccessModal() {
        setSuccessModalActive(false);
        resetValues();
    }

    return (
        <>
            <form className='login-form' onSubmit={onSubmit}>
                <ul className="form__inputs">
                    <Input
                        type={InputType.password}
                        name="old-pass"
                        onChange={onChange}
                        placeholder="Введите старый пароль..."
                        value={oldPassword}
                        errorMessage={oldPasswordError}
                    />
                    <Input
                        type={InputType.password}
                        name="new-pass"
                        onChange={onChange}
                        placeholder="Введите новый пароль..."
                        value={newPassword}
                        errorMessage={newPasswordError}
                    />
                </ul>
                <nav className="form__nav">
                    <button className="button-violet" type="submit">Сохранить</button>
                </nav>
            </form >

            <ErrorModal active={isErrorModalActive} error={error} onClose={closeErrorModal} />
            <SuccessModal active={isSuccessModalActive} message={success} onClose={closeSuccessModal} />
        </>
    );
}

export default EditPassForm;