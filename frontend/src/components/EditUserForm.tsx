import { useState } from "react";
import { User } from "../models/User";

interface IRegisterProps {
    defaultValues: User;
    onSubmit(user: User): Promise<void>;
}
export function EditUserForm({ defaultValues, onSubmit }: IRegisterProps) {
    const [nickname, setNickname] = useState(defaultValues.nickname);
    const [lastname, setLastname] = useState(defaultValues.lastname);
    const [name, setName] = useState(defaultValues.name);

    const submitHandler = async (event: React.FormEvent) => {
        event.preventDefault();

        const user = new User(nickname, lastname, name, defaultValues.id);

        console.log(user);

        await onSubmit(user);
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

    return (
        <form className='login-form' onSubmit={submitHandler}>
            <ul className="form__inputs">
                <p className="medium-little">Никнейм</p>
                <li className="login-input">
                    <input
                        className={'violet-input'}
                        type="text" name="nickname"
                        placeholder="Введите никнейм..."
                        style={{ width: '300px' }}
                        onChange={onChange}
                        value={nickname}
                    />
                </li>

                <p className="medium-little">Фамилия</p>
                <li className="login-input">
                    <input
                        className={'violet-input'}
                        type="text" name="lastname"
                        placeholder="Введите фамилию..."
                        style={{ width: '300px' }}
                        onChange={onChange}
                        value={lastname}
                    />
                </li>

                <p className="medium-little">Имя</p>
                <li className="login-input">
                    <input
                        className={'violet-input'}
                        type="text" name="name"
                        placeholder="Введите имя..."
                        style={{ width: '300px' }}
                        onChange={onChange}
                        value={name}
                    />
                </li>
            </ul>
            <nav className="form__nav">
                <button className="button-violet" type="submit">Сохранить</button>
            </nav>
        </form >
    );
}