import { useState } from "react";
import MainHeaderViaNav from "../components/ContainerHeaderViaNav";
import { Modal } from "../components/Modal";
import { Input } from "../ui/Input";
import { InputType } from "../models/enums";
import { validate } from "../helpers/validation";
import Checkbox from "../ui/Checkbox";

function UserCourseCreator() {
    const [createModalActive, setCreateModalActive] = useState(false);

    const [courseTitle, setCourseTitle] = useState('');
    const [courseTitleError, setCourseTitleError] = useState('');

    const [courseDescription, setCourseDescription] = useState('');

    async function onSubmit(event: React.FormEvent) {
        event.preventDefault();

        let isValidDate = true;


        if (!validate(courseTitle)) {
            setCourseTitleError("Название курса должно быть заполнено");
            isValidDate = false;
        }
        if (!isValidDate) return;
    }

    function onChange(event: React.ChangeEvent) {
        const inputElement = event.target as HTMLInputElement;

        switch (inputElement.name) {
            case 'courseTitle':
                setCourseTitle(inputElement.value);
                setCourseTitleError('');
                break;
            case 'courseDescription':
                setCourseDescription(inputElement.value);
                break;
        }
    }


    return (
        <>
            <MainHeaderViaNav title="Мои доступные курсы">
                <button className="button-gray-violet" style={{ width: '50px', height: '50px' }} onClick={() => setCreateModalActive(true)}>

                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={2.2} stroke="currentColor" style={{ width: '30px', height: '30px' }}>
                        <path strokeLinecap="round" strokeLinejoin="round" d="M12 4.5v15m7.5-7.5h-15" />
                    </svg>
                </button>
            </MainHeaderViaNav>

            <Modal active={createModalActive} title="Создание курса" onClose={() => setCreateModalActive(false)}>
                <form className="base-form" onSubmit={onSubmit}>
                    <ul className="form__inputs">
                        <p className="medium-little">Название курса</p>
                        <Input
                            type={InputType.text}
                            name="courseTitle"
                            onChange={onChange}
                            errorMessage={courseTitleError}
                            placeholder="Введите название курса..."
                            width={500}
                        />

                        <p className="medium-little">Описание курса</p>
                        <textarea
                            name="courseTitle"
                            onChange={onChange}
                            placeholder="Введите описание курса (Необязательно)..."
                            style={{ width: '500px' }}
                            className="rich-text"
                        />
                        <Checkbox label="Hello" isChecked={false} />
                    </ul>
                    <nav className="form__nav">
                        <button className="button-violet" type="submit">Создать</button>
                    </nav>
                </form>
            </Modal >
        </>
    );
}

export default UserCourseCreator;