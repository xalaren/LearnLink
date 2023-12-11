import { useEffect, useState } from "react";
import { User } from "../models/user";
import { useAppDispatch, useAppSelector } from "../hooks/redux";
import { Input } from "../ui/Input";
import { InputType } from "../helpers/enums";
import { useUpdateUserData } from "../hooks/userHooks";
import { loginSave } from "../store/actions/authActionCreators";
import { fetchUser } from "../store/actions/userActionCreators";
import { ErrorModal } from "../components/ErrorModal";
import { SuccessModal } from "../components/SuccessModal";

export function EditUserForm() {
    const { accessToken } = useAppSelector(state => state.authReducer);
    const { user } = useAppSelector(state => state.userReducer);
    const { updateUserDataQuery, token, error, success, resetValues } = useUpdateUserData();
    const dispatch = useAppDispatch();

    const [nickname, setNickname] = useState('');
    const [lastname, setLastname] = useState('');
    const [name, setName] = useState('');

    const [isErrorModalActive, setErrorModalActive] = useState(false);
    const [isSuccessModalActive, setSuccessModalActive] = useState(false);


    useEffect(() => {
        setNickname(user?.nickname || '');
        setLastname(user?.lastname || '');
        setName(user?.name || '');
    }, [user]);

    useEffect(() => {
        if (error) {
            setErrorModalActive(true);
        }

        if (success && token) {
            dispatch(loginSave(token, nickname));
            dispatch(fetchUser());
            setSuccessModalActive(true);
        }
    }, [dispatch, error, nickname, success, token]);

    const onSubmit = async (event: React.FormEvent) => {
        event.preventDefault();

        const newUser = new User(nickname, lastname, name, user?.role, user?.id);

        await updateUserDataQuery(newUser, accessToken);
    }

    function onChange(event: React.ChangeEvent) {
        const inputElement = event.target as HTMLInputElement;

        switch (inputElement.name) {
            case 'nickname':
                setNickname(inputElement.value);
                break;
            case 'lastname':
                setLastname(inputElement.value);
                break;
            case 'name':
                setName(inputElement.value);
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
    }

    return (
        <>

            <form className='login-form' onSubmit={onSubmit}>
                <ul className="form__inputs">
                    <p className="medium-little">Никнейм</p>
                    <Input
                        type={InputType.text}
                        name="nickname"
                        onChange={onChange}
                        placeholder="Введите никнейм..."
                        value={nickname}
                    />

                    <p className="medium-little">Фамилия</p>
                    <Input
                        type={InputType.text}
                        name="lastname"
                        onChange={onChange}
                        placeholder="Введите фамилию..."
                        value={lastname} />

                    <p className="medium-little">Имя</p>
                    <Input
                        type={InputType.text}
                        name="name"
                        onChange={onChange}
                        placeholder="Введите имя..."
                        value={name} />
                </ul>
                <nav className="form__nav">
                    <button className="button-violet" type="submit">Сохранить</button>
                </nav>
            </form >

            <ErrorModal active={isErrorModalActive} error={error} onClose={closeErrorModal} />
            <SuccessModal active={isSuccessModalActive} message={success} onClose={closeSuccessModal} />
        </>
    );
}