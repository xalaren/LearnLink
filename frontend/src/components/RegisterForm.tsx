import { useState } from "react";
import { validate } from "../services/Validation";
import { User } from "../models/User";

interface IRegisterProps {
    onSubmit(user: User, password: string): Promise<void>;
}

export function RegisterForm({ onSubmit }: IRegisterProps) {
    const [nickname, setNickname] = useState('');
    const [password, setPassword] = useState('');
    const [lastname, setLastname] = useState('');
    const [name, setName] = useState('');
    const [nicknameError, setNicknameError] = useState('');
    const [passwordError, setPasswordError] = useState('');
    const [nameError, setNameError] = useState('');
    const [lastnameError, setLastnameError] = useState('');

    const submitHandler = async (event: React.FormEvent) => {
        event.preventDefault();

        let validationError: boolean = false;
        if (!validate(nickname)) {
            setNicknameError("Никнейм должен быть заполнен");
            validationError = true;
        }

        if (!validate(password)) {
            setPasswordError("Пароль должен быть заполнен");
            validationError = true;
        }

        if (!validate(lastname)) {
            setLastnameError("Фамилия должна быть заполнена");
            validationError = true;
        }

        if (!validate(name)) {
            setNameError('Имя должно быть заполнено');
            validationError = true;
        }

        console.log(validationError);

        if (validationError) {
            return;
        }

        const user = new User(nickname, lastname, name);

        await onSubmit(user, password);
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
            case 'lastname':
                setLastname(inputElement.value);
                setLastnameError('');
                break;
            case 'name':
                setName(inputElement.value);
                setNameError('');
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
                <li className="login-input">
                    {lastnameError &&
                        <div className="regular-red required">{lastnameError}</div>
                    }
                    <input
                        className={lastnameError ? 'red-input' : 'violet-input'}
                        type="text" name="lastname"
                        placeholder="Введите фамилию..."
                        style={{ width: '300px' }}
                        onChange={(event) => {
                            setLastname(event.target.value);
                            setLastnameError('');
                        }} />
                </li>
                <li className="login-input">
                    {nameError &&
                        <div className="regular-red required">{nameError}</div>
                    }
                    <input
                        className={nameError ? 'red-input' : 'violet-input'}
                        type="text" name="name"
                        placeholder="Введите имя..."
                        style={{ width: '300px' }}
                        onChange={(event) => {
                            setName(event.target.value);
                            setNameError('');
                        }} />
                </li>
            </ul>
            <nav className="form__nav">
                <button className="button-orange" type="submit">Регистрация</button>
            </nav>
        </form >
    );
}