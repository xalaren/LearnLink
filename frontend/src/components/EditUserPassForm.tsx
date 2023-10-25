import { useState } from "react";
import { validate } from "../services/Validation";

interface ILoginFormProps {
    onSubmit(oldPassword: string, newPassword: string): Promise<void>;
}

export function EditUserPassForm({ onSubmit }: ILoginFormProps) {
    const [oldPassword, setOldPassword] = useState('');
    const [newPassword, setNewPassword] = useState('');
    const [oldPasswordError, setOldPasswordError] = useState('');
    const [newPasswordError, setNewPasswordError] = useState('');

    const submitHandler = (event: React.FormEvent) => {
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

        onSubmit(oldPassword, newPassword);
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

    return (
        <form className='login-form' onSubmit={submitHandler}>
            <ul className="form__inputs">
                <li className="login-input">
                    {oldPasswordError &&
                        <div className="regular-red required">{oldPasswordError}</div>
                    }
                    <input
                        className={oldPasswordError ? 'red-input' : 'violet-input'}
                        type="text" name="old-pass"
                        placeholder="Введите старый пароль..."
                        style={{ width: '300px' }}
                        onChange={onChange} />
                </li>
                <li className="login-input">
                    {newPasswordError &&
                        <div className="regular-red required">{newPasswordError}</div>
                    }
                    <input
                        className={newPasswordError ? 'red-input' : 'violet-input'}
                        type="password" name="new-pass"
                        placeholder="Введите новый пароль..."
                        style={{ width: '300px' }}
                        onChange={onChange} />
                </li>
            </ul>
            <nav className="form__nav">
                <button className="button-orange" type="submit">Сохранить</button>
            </nav>
        </form >
    );
}