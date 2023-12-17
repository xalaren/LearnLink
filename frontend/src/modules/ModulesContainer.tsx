import { useState } from "react";
import { Input } from "../ui/Input";
import { InputType } from "../models/enums";

interface IModulesContainerProps {
    allowEdit: boolean;
}

function ModulesContainer({ allowEdit }: IModulesContainerProps) {
    const [inputActive, setInputActive] = useState(false);
    const [moduleTitle, setModuleTitle] = useState('');
    const [moduleTitleError, setModuleTitleError] = useState('');

    async function onSubmit(event: React.FormEvent) {
        event.preventDefault();
    }

    function onChange(event: React.ChangeEvent) {
        const inputElement = event.target as HTMLInputElement;

        switch (inputElement.name) {
            case 'moduleTitle':
                setModuleTitle(inputElement.value);
                setModuleTitleError('');
                break;
        }
    }

    return (
        <div className="course-view__modules">
            <div className="modules__header">
                <h3>Изучаемые модули</h3>
                <ul className="modules__container">

                </ul>


                {allowEdit && <form className='base-form' onSubmit={onSubmit}>
                    <Input
                        name="moduleTitle"
                        onChange={onChange}
                        type={InputType.text}
                        width={400}
                        placeholder="Введите название модуля..."
                    />
                </form>
                }
            </div>
        </div>
    );
}

export default ModulesContainer;