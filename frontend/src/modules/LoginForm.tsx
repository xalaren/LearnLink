import { useEffect, useState } from "react";
import { InputType } from "../helpers/enums";
import { validate } from "../helpers/validation";
import { Input } from "../ui/Input";
import { useLogin } from "../hooks/userHooks";
import { ErrorModal } from "../components/ErrorModal";
import { useAppDispatch } from "../hooks/redux";
import { loginSave } from "../store/actions/authActionCreators";

export function LoginForm() {
    const [nickname, setNickname] = useState('');
    const [password, setPassword] = useState('');
    const [nicknameError, setNicknameError] = useState('');
    const [passwordError, setPasswordError] = useState('');
    const [isModalActive, setModalActive] = useState(false);
    const dispatch = useAppDispatch();

    const { loginQuery, error, resetError, authModel } = useLogin();

    useEffect(() => {
        if (error) {
            setModalActive(true);
        }

        if (authModel) {
            dispatch(loginSave(authModel.accessToken, authModel.nickname));
        }
    }, [error, authModel, dispatch]);

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

        await loginQuery(nickname, password);
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

    function closeModal() {
        setModalActive(false);
        resetError();
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

            <ErrorModal active={isModalActive} error={error} onClose={closeModal} />
        </>
    );


}