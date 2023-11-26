import { useState } from "react";
import { validate } from "../services/Validation";

interface ILoginFormProps {
    onSubmit(nickname: string, password: string): Promise<any>;
}

export function LoginForm({ onSubmit }: ILoginFormProps) {
    const [nickname, setNickname] = useState('');
    const [password, setPassword] = useState('');
    const [nicknameError, setNicknameError] = useState('');
    const [passwordError, setPasswordError] = useState('');

    const submitHandler = (event: React.FormEvent) => {
        let validationError: boolean = false;

        event.preventDefault();

        if (!validate(nickname)) {
            setNicknameError("Никнейм должен быть заполнен");
            validationError = true;
        }

        if (!validate(password)) {
            setPasswordError("Пароль должен быть заполнен");
            validationError = true;
        }

        if (validationError) return;

        onSubmit(nickname, password);
    }

    function onChange(event: React.ChangeEvent) {
        const inputElement = event.target as HTMLInputElement;

        switch (inputElement.name) {
            case 'nickname':
                setNickname(inputElement.value);
                setNicknameError('');
                break;
            case 'password':
                setPassword(inputElement.value);
                setPasswordError('');
                break;
        }
    }

    return (
        <form className='login-form' onSubmit={submitHandler}>
            <ul className="form__inputs">
                <li className="login-input">
                    {nicknameError &&
                        <div className="regular-red required">{nicknameError}</div>
                    }
                    <input
                        className={nicknameError ? 'red-input' : 'violet-input'}
                        type="text" name="nickname"
                        placeholder="Введите никнейм..."
                        style={{ width: '300px' }}
                        onChange={onChange} />
                </li>
                <li className="login-input">
                    {passwordError &&
                        <div className="regular-red required">{passwordError}</div>
                    }
                    <input
                        className={passwordError ? 'red-input' : 'violet-input'}
                        type="password" name="password"
                        placeholder="Введите пароль..."
                        style={{ width: '300px' }}
                        onChange={onChange} />
                </li>
            </ul>
            <nav className="form__nav">
                <button className="button-orange" type="submit">Войти</button>
            </nav>
        </form >
    );
}