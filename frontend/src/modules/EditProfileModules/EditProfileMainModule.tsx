import { useAppDispatch, useAppSelector } from "../../hooks/redux.ts";
import { useUpdateUserData } from "../../hooks/userHooks.ts";
import ProfileCard from "../../components/ProfileCard.tsx";
import { Input } from "../../components/Input.tsx";
import { InputType, NotificationType } from "../../models/enums.ts";
import { useEffect, useState } from "react";
import { validate } from "../../helpers/validation.ts";
import { loginSave } from "../../store/actions/authActionCreators.ts";
import { fetchUser } from "../../store/actions/userActionCreators.ts";
import { User } from "../../models/user.ts";
import PopupNotification from "../../components/PopupNotification.tsx";

function EditProfileMainModule() {
    const { accessToken } = useAppSelector(state => state.authReducer);
    const { user } = useAppSelector(state => state.userReducer);
    const dispatch = useAppDispatch();

    const { updateUserDataQuery, token, error, success, resetValues } = useUpdateUserData();

    const [nickname, setNickname] = useState('');
    const [lastname, setLastname] = useState('');
    const [name, setName] = useState('');
    const [uploadedImage, setUploadedImage] = useState<File>();
    const [nicknameError, setNicknameError] = useState('');
    const [nameError, setNameError] = useState('');
    const [lastnameError, setLastnameError] = useState('');

    useEffect(() => {
        setNickname(user?.nickname || '');
        setLastname(user?.lastname || '');
        setName(user?.name || '');
    }, [user]);

    useEffect(() => {
        if (success && token) {
            dispatch(loginSave(token, nickname));
            dispatch(fetchUser());
        }
    }, [dispatch, nickname, token]);


    async function onSubmit(event: React.FormEvent) {
        event.preventDefault();

        let isValidDate = true;


        if (!validate(nickname)) {
            setNicknameError("Никнейм должен быть заполнен");
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

        if (user) {
            const newUser = new User(nickname, lastname, name, uploadedImage, '', user?.role, user?.id);
            await updateUserDataQuery(newUser, accessToken);
        }
    }

    async function onChange(event: React.ChangeEvent) {
        const inputElement = event.target as HTMLInputElement;

        switch (inputElement.name) {
            case 'nickname':
                setNickname(inputElement.value);
                setNicknameError('');
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
        <>
            <h3 className="account-page__title">Основные данные</h3>

            {user &&
                <form className="form-bordered account-change-form" onSubmit={onSubmit}>

                    <ProfileCard
                        inputName='avatar'
                        user={user}
                        onImageChange={onChange}
                        profileImage={uploadedImage}
                    />

                    <div className="form__inputs">
                        <Input
                            type={InputType.text}
                            className="form__form-input"
                            name="nickname"
                            errorMessage={nicknameError}
                            placeholder="Введите никнейм..."
                            label="Никнейм"
                            onChange={onChange}
                            value={nickname}
                            required={true}
                        />

                        <Input
                            type={InputType.text}
                            className="form__form-input"
                            name="name"
                            errorMessage={nameError}
                            placeholder="Введите имя..."
                            label="Имя"
                            onChange={onChange}
                            value={name}
                            required={true}
                        />

                        <Input
                            type={InputType.text}
                            className="form__form-input"
                            name="lastname"
                            errorMessage={lastnameError}
                            placeholder="Введите фамилию..."
                            label="Фамилия"
                            onChange={onChange}
                            value={lastname}
                            required={true}
                        />
                    </div>

                    <nav className="form__nav">
                        <button type="submit" className="form__button button-violet">Сохранить изменения</button>
                    </nav>
                </form>

            }

            {success &&
                <PopupNotification notificationType={NotificationType.success} onFade={resetValues}>
                    {success}
                </PopupNotification>
            }

            {error &&
                <PopupNotification notificationType={NotificationType.error} onFade={resetValues}>
                    {error}
                </PopupNotification>
            }

        </>
    );
}

export default EditProfileMainModule;