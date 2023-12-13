import { useEffect, useState } from "react";
import { ErrorModal } from "../components/ErrorModal";
import { validate } from "../helpers/validation";
import { Input } from "../ui/Input";
import { InputType, Paths } from "../models/enums";
import { useRegister } from "../hooks/userHooks";
import { SuccessModal } from "../components/SuccessModal";
import { useHistoryNavigation } from "../hooks/historyNavigation";

export function RegisterForm() {
    const [nickname, setNickname] = useState('');
    const [password, setPassword] = useState('');
    const [lastname, setLastname] = useState('');
    const [name, setName] = useState('');
    const [nicknameError, setNicknameError] = useState('');
    const [passwordError, setPasswordError] = useState('');
    const [nameError, setNameError] = useState('');
    const [lastnameError, setLastnameError] = useState('');

    const [isErrorModalActive, setErrorModalActive] = useState(false);
    const [isSuccessModalActive, setSuccessModalActive] = useState(false);

    const { registerQuery, error, success, resetValues } = useRegister();
    const { toNext } = useHistoryNavigation();

    useEffect(() => {
        if (error) {
            setErrorModalActive(true);
        }

        if (success) {
            setSuccessModalActive(true);
        }
    }, [error, success])

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

        if (!validate(lastname)) {
            setLastnameError("Фамилия должна быть заполнена");
            isValidDate = false;
        }

        if (!validate(name)) {
            setNameError("Имя должно быть заполнено");
            isValidDate = false;
        }

        if (!isValidDate) return;

        await registerQuery(nickname, password, lastname, name);

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

    function closeErrorModal() {
        setErrorModalActive(false);
        resetValues();
    }

    function closeSuccessModal() {
        setSuccessModalActive(false);
        resetValues();
        toNext(Paths.loginPath);
    }

    return (
        <>
            <form className='base-form' onSubmit={onSubmit}>
                <ul className="form__inputs">
                    <p className="medium-little">Никнейм</p>
                    <Input
                        type={InputType.text}
                        name="nickname"
                        onChange={onChange}
                        errorMessage={nicknameError}
                        placeholder="Введите никнейм..."
                    />

                    <p className="medium-little">Пароль</p>
                    <Input
                        type={InputType.password}
                        name="password"
                        onChange={onChange}
                        errorMessage={passwordError}
                        placeholder="Введите пароль..."
                    />

                    <p className="medium-little">Фамилия</p>
                    <Input
                        type={InputType.text}
                        name="lastname"
                        onChange={onChange}
                        errorMessage={lastnameError}
                        placeholder="Введите фамилию..."
                    />

                    <p className="medium-little">Имя</p>
                    <Input
                        type={InputType.text}
                        name="name"
                        onChange={onChange}
                        errorMessage={nameError}
                        placeholder="Введите имя..."
                    />
                </ul>
                <nav className="form__nav">
                    <button className="button-violet" type="submit">Регистрация</button>
                </nav>
            </form >

            <ErrorModal active={isErrorModalActive} error={error} onClose={closeErrorModal} />
            <SuccessModal active={isSuccessModalActive} message={success} onClose={closeSuccessModal} />
        </>
    );
}