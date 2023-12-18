import { useEffect, useState } from "react";
import { Input } from "../ui/Input";
import { InputType } from "../models/enums";
import { PlusIcon } from "../ui/PlusIcon";
import CrossIcon from "../ui/CrossIcon";
import CheckIcon from "../ui/CheckIcon";
import { useGetCourseModules } from "../hooks/moduleHooks";
import { useAppSelector } from "../hooks/redux";
import { ErrorModal } from "../components/ErrorModal";
import PopupLoader from "../ui/PopupLoader";
import { Loader } from "../ui/Loader";
import ModuleItem from "../ui/ModuleItem";

interface IModulesContainerProps {
    allowEdit: boolean;
    courseId: number;
}

function ModulesContainer({ allowEdit, courseId }: IModulesContainerProps) {
    const [inputActive, setInputActive] = useState(false);
    const [moduleTitle, setModuleTitle] = useState('');
    const [moduleTitleError, setModuleTitleError] = useState('');

    const [localError, setLocalError] = useState('');
    const [localLoading, setLocalLoading] = useState(false);

    const { getModulesQuery, modules, error: getModulesError, loading: getModulesLoading } = useGetCourseModules();

    const { accessToken } = useAppSelector(state => state.authReducer);

    useEffect(() => {
        fetchModules();
    }, [accessToken]);

    useEffect(() => {
        if (getModulesError) setLocalError(getModulesError);
    }, [getModulesError]);

    useEffect(() => {
        setLocalLoading(getModulesLoading);
    }, [getModulesLoading])

    async function fetchModules() {
        if (accessToken) await getModulesQuery(courseId, accessToken);
    }

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

    function resetLocalError() {
        setLocalError('');
    }

    return (
        <div className="course-view__modules">
            <section className="modules__header container__header">
                <h3>Изучаемые модули</h3>
                {allowEdit && !inputActive &&
                    <div className="container__navigation">
                        <button className="button-gray-violet" style={{ width: '50px', height: '50px' }} onClick={() => { setInputActive(true) }}>
                            <PlusIcon />
                        </button>
                    </div>
                }
            </section >
            <ul className="modules__container">
                {localLoading && <Loader />}
                {modules && modules.map(module => <ModuleItem module={module} onClick={() => { }} />)
                }
                {allowEdit && inputActive &&
                    <form className='base-form' onSubmit={onSubmit}>
                        <div className="module-controls">
                            <Input
                                name="moduleTitle"
                                onChange={onChange}
                                type={InputType.text}
                                width={400}
                                placeholder="Введите название модуля..."
                                className="line-input"
                            />
                            <button className="transparent-red-button" type="button" onClick={() => { setInputActive(false) }}>
                                <CrossIcon />
                            </button>

                            <button className="transparent-green-button" type="submit">
                                <CheckIcon />
                            </button>
                        </div>

                    </form>
                }
            </ul>


            <ErrorModal active={Boolean(localError)} error={localError} onClose={resetLocalError} />
        </div >
    );
}

export default ModulesContainer;