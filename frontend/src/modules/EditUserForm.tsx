import { useState } from "react";
import { User } from "../models/user";
import { useAppSelector } from "../hooks/redux";
import { Input } from "../ui/Input";
import { InputType } from "../helpers/enums";

export function EditUserForm() {
    const user = useAppSelector(state => state.userReducer.user);

    const [nickname, setNickname] = useState(user?.nickname || '');
    const [lastname, setLastname] = useState(user?.lastname || '');
    const [name, setName] = useState(user?.name || '');

    const submitHandler = async (event: React.FormEvent) => {
        event.preventDefault();

        const newUser = new User(nickname, lastname, name, user?.role, user?.id);

        console.log(newUser);
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
                <Input
                    type={InputType.text}
                    name="nickname"
                    onChange={onChange}>
                    Введите никнейм...
                </Input>

                <p className="medium-little">Фамилия</p>
                <Input
                    type={InputType.text}
                    name="lastname"
                    onChange={onChange}>
                    Введите фамилию...
                </Input>

                <p className="medium-little">Имя</p>
                <Input
                    type={InputType.text}
                    name="name"
                    onChange={onChange}>
                    Введите имя...
                </Input>
            </ul>
            <nav className="form__nav">
                <button className="button-violet" type="submit">Сохранить</button>
            </nav>
        </form >
    );
}