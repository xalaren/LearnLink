import { useEffect, useState } from "react";
import { useAppDispatch, useAppSelector } from "../hooks/redux";
import { useHistoryNavigation } from "../hooks/historyNavigation";
import { MainContainer } from "../components/MainContainer";
import { Input } from "../ui/Input";
import { InputType } from "../models/enums";
import { validate } from "../helpers/validation";
import { useLogin } from "../hooks/userHooks";
import { ErrorModal } from "../components/ErrorModal";
import { loginSave } from "../store/actions/authActionCreators";
import { fetchUser } from "../store/actions/userActionCreators";


export function LoginPage() {
    const isAuthenticated = useAppSelector(state => state.authReducer.isAuthenticated);
    const { toPrev } = useHistoryNavigation();

    const [nickname, setNickname] = useState('');
    const [password, setPassword] = useState('');
    const [nicknameError, setNicknameError] = useState('');
    const [passwordError, setPasswordError] = useState('');

    const { loginQuery, error, resetError } = useLogin();

    const dispatch = useAppDispatch();

    useEffect(() => {
        if (isAuthenticated) toPrev();
    }, [isAuthenticated])

    useEffect(() => {
        if (error) {
            setNickname('');
            setPassword('');
        }
    }, [error])


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

        const authModel = await loginQuery(nickname, password);

        if (authModel) {
            dispatch(loginSave(authModel.accessToken, authModel.nickname));
            dispatch(fetchUser());
            toPrev();
        }
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
        <MainContainer className="auth-page">
            <form className="auth-page__base-form base-form" onSubmit={onSubmit}>
                <h3 className="base-form__title">Вход в систему</h3>

                <div className="base-form__inputs">
                    <Input
                        type={InputType.text}
                        className="base-form__form-input"
                        name="nickname"
                        errorMessage={nicknameError}
                        placeholder="Введите никнейм..."
                        label="Никнейм"
                        value={nickname}
                        onChange={onChange}
                    />

                    <Input
                        type={InputType.password}
                        className="base-form__form-input"
                        name="password"
                        errorMessage={passwordError}
                        placeholder="Введите пароль..."
                        label="Пароль"
                        value={password}
                        onChange={onChange}
                    />
                </div>

                <button type="submit" className="base-form__button button-violet">Войти</button>
            </form>

            <ErrorModal active={Boolean(error)} onClose={resetError} error={error} />
        </MainContainer>
    )
}