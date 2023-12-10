import { useEffect, useState } from "react";
import { ErrorModal } from "../components/ErrorModal";
import { validate } from "../helpers/validation";
import { Input } from "../ui/Input";
import { InputType } from "../helpers/enums";

export function RegisterForm() {
    const [nickname, setNickname] = useState('');
    const [password, setPassword] = useState('');
    const [nicknameError, setNicknameError] = useState('');
    const [passwordError, setPasswordError] = useState('');

    async function onSubmit(event: React.FormEvent) {
        event.preventDefault();

        let isValidDate = true;


        if (!validate(nickname)) {
            setNicknameError("Никнейм должен быть заполнен");
            isValidDate = false;
        }

        if (!validate(password)) {
            setPasswordError("Пароль должен быть заполнен");
            isValidDate = false;
        }

        if (!isValidDate) return;

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
        <>
            <form className='login-form' onSubmit={onSubmit}>
                <ul className="form__inputs">
                    <Input
                        type={InputType.text}
                        name="nickname"
                        onChange={onChange}
                        errorMessage={nicknameError}>
                        Введите никнейм...
                    </Input>
                    <Input
                        type={InputType.password}
                        name="password"
                        onChange={onChange}
                        errorMessage={passwordError}>
                        Введите пароль...
                    </Input>
                </ul>
                <nav className="form__nav">
                    <button className="button-violet" type="submit">Войти</button>
                </nav>
            </form >
        </>
    );
}