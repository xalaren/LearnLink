import { useEffect, useState } from "react";
import { MainContainer } from "../components/MainContainer";
import { useAppSelector } from "../hooks/redux";
import { useHistoryNavigation } from "../hooks/historyNavigation";
import { Paths } from "../models/paths";
import { Input } from "../ui/Input";
import { InputType } from "../models/enums";
import ImageUploader from "../ui/ImageUploader";
import { validate } from "../helpers/validation";
import { useRegister } from "../hooks/userHooks";
import { ErrorModal } from "../components/ErrorModal";
import { SuccessModal } from "../components/SuccessModal";


export function RegisterPage() {
    const isAuthenticated = useAppSelector(state => state.authReducer.isAuthenticated);
    const { toNext } = useHistoryNavigation();

    const [nickname, setNickname] = useState('');
    const [password, setPassword] = useState('');
    const [lastname, setLastname] = useState('');
    const [name, setName] = useState('');
    const [uploadedImage, setUploadedImage] = useState<File>();
    const [nicknameError, setNicknameError] = useState('');
    const [passwordError, setPasswordError] = useState('');
    const [nameError, setNameError] = useState('');
    const [lastnameError, setLastnameError] = useState('');

    const { registerQuery, error, success, resetValues } = useRegister();

    useEffect(() => {
        if (isAuthenticated) toNext(Paths.homePath);
    }, [isAuthenticated, toNext]);

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

        await registerQuery(nickname, password, lastname, name, uploadedImage);

        if (success) {
            toNext(Paths.loginPath);
        }
    }

    async function onChange(event: React.ChangeEvent) {
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
            case 'avatar':
                if (inputElement.files) {
                    setUploadedImage(inputElement.files[0]);
                }
                break;
        }
    }

    return (
        <MainContainer className="auth-page">
            <form className="auth-page__base-form base-form" onSubmit={onSubmit}>
                <h3 className="base-form__title">Регистрация</h3>

                <ImageUploader
                    name="avatar"
                    className="base-form__image-uploader"
                    onChange={onChange}
                    image={uploadedImage}
                />

                <div className="base-form__inputs">
                    <Input
                        type={InputType.text}
                        className="base-form__form-input"
                        name="nickname"
                        errorMessage={nicknameError}
                        placeholder="Введите никнейм..."
                        label="Никнейм"
                        onChange={onChange}
                    />

                    <Input
                        type={InputType.password}
                        className="base-form__form-input"
                        name="password"
                        errorMessage={passwordError}
                        placeholder="Введите пароль..."
                        label="Пароль"
                        onChange={onChange}
                    />

                    <Input
                        type={InputType.text}
                        className="base-form__form-input"
                        name="name"
                        errorMessage={nameError}
                        placeholder="Введите имя..."
                        label="Имя"
                        onChange={onChange}
                    />

                    <Input
                        type={InputType.text}
                        className="base-form__form-input"
                        name="lastname"
                        errorMessage={lastnameError}
                        placeholder="Введите фамилию..."
                        label="Фамилия"
                        onChange={onChange}
                    />
                </div>

                <button type="submit" className="base-form__button button-violet">Зарегистрироваться</button>
            </form>

            <ErrorModal active={Boolean(error)} onClose={resetValues} error={error} />
            <SuccessModal active={Boolean(success)} onClose={() => { toNext(Paths.loginPath) }} message={success} />
        </MainContainer>
    )
}