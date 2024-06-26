import { useState } from "react";
import { MainContainer } from "../components/MainContainer";
import { useHistoryNavigation } from "../hooks/historyNavigation";
import { paths } from "../models/paths";
import { Input } from "../components/Input/Input";
import { InputType } from "../models/enums";
import ImageUploader from "../components/ImageUploader/ImageUploader";
import { validate } from "../helpers/validation";
import { useRegister } from "../hooks/userHooks";
import { ErrorModal } from "../components/Modal/ErrorModal";
import { SuccessModal } from "../components/Modal/SuccessModal";
import Checkbox from "../components/Checkbox";
import { Link } from "react-router-dom";


export function RegisterPage() {
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
    const [privacyAccept, setPrivacyAccept] = useState(false);
    const [privacyAcceptError, setPrivacyAcceptError] = useState('');

    const { registerQuery, error, success, resetValues } = useRegister();

    function resetDefault() {
        resetValues();
        setPrivacyAcceptError('');
    }

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

        if (!privacyAccept) {
            setPrivacyAcceptError("Вы не приняли пользовательское соглашение");
            isValidDate = false;
        }

        if (!isValidDate) return;

        await registerQuery(nickname, password, lastname, name, uploadedImage);

        if (success) {
            toNext(paths.login);
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
            <form className="auth-page__form form-bordered" onSubmit={onSubmit}>
                <h3 className="form__title">Регистрация</h3>



                <div className="form__inputs">

                    <ImageUploader
                        name="avatar"
                        className="form__image-uploader"
                        onChange={onChange}
                        image={uploadedImage}
                    />

                    <Input
                        type={InputType.text}
                        className="form__form-input input-gray"
                        name="nickname"
                        errorMessage={nicknameError}
                        placeholder="Введите никнейм..."
                        label="Никнейм"
                        onChange={onChange}
                        required={true}
                    />

                    <Input
                        type={InputType.password}
                        className="form__form-input input-gray"
                        name="password"
                        errorMessage={passwordError}
                        placeholder="Введите пароль..."
                        label="Пароль"
                        onChange={onChange}
                        required={true}
                    />

                    <Input
                        type={InputType.text}
                        className="form__form-input input-gray"
                        name="name"
                        errorMessage={nameError}
                        placeholder="Введите имя..."
                        label="Имя"
                        onChange={onChange}
                        required={true}
                    />

                    <Input
                        type={InputType.text}
                        className="form__form-input input-gray"
                        name="lastname"
                        errorMessage={lastnameError}
                        placeholder="Введите фамилию..."
                        label="Фамилия"
                        onChange={onChange}
                        required={true}
                    />


                    <Checkbox
                        isChecked={privacyAccept}
                        checkedChanger={() => { setPrivacyAccept(prev => !prev); }}
                    >
                        <p className={`form-input__label-required ui-text`}>
                            Я даю согласие на обработку моих <Link to={paths.privacy.full}>персональных данных</Link>
                        </p>
                    </Checkbox>

                </div>

                <button type="submit" className="form__button button-violet">Зарегистрироваться</button>
            </form>

            <ErrorModal active={Boolean(error || privacyAcceptError)} onClose={resetDefault} error={error || privacyAcceptError} />
            <SuccessModal active={Boolean(success)} onClose={() => { toNext(paths.login) }} message={success} />
        </MainContainer >
    )
}