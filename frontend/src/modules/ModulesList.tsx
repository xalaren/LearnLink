import { useEffect, useState } from "react";
import { Course } from "../models/course";
import { useAppSelector } from "../hooks/redux";
import { Module } from "../models/module";
import ItemLink from "../components/ItemLink";
import ContentList from "../components/ContentList/ContentList";
import { useCreateModules, useGetCourseModules } from "../hooks/moduleHooks";
import { Loader } from "../components/Loader/Loader";
import { ErrorModal } from "../components/Modal/ErrorModal";
import { useHistoryNavigation } from "../hooks/historyNavigation";
import { Paths } from "../models/paths";
import { validate } from "../helpers/validation";
import { Modal } from "../components/Modal/Modal";
import ModalContent from "../components/Modal/ModalContent";
import { Input } from "../components/Input";
import { InputType, NotificationType } from "../models/enums";
import ModalFooter from "../components/Modal/ModalFooter";
import ModalButton from "../components/Modal/ModalButton";
import PopupLoader from "../components/Loader/PopupLoader";
import PopupNotification from "../components/PopupNotification";

interface IModuleListProps {
    course: Course;
}

function ModulesList({ course }: IModuleListProps) {
    const { user } = useAppSelector(state => state.userReducer);
    const [modules, setModules] = useState<Module[]>();
    const { getModulesQuery, error, loading, resetValues } = useGetCourseModules();

    const [createModalActive, setCreateModalActive] = useState(false);

    useEffect(() => {
        fetchModules();
    }, [user, createModalActive]);

    async function fetchModules() {
        if (!user) return;

        const foundModules = await getModulesQuery(course.id, user.id);

        if (foundModules) {
            setModules(foundModules);
        }
    }

    return (
        <>
            <ContentList
                className="content-side__main"
                title="Изучаемые модули"
                showButton={course.localRole?.manageInternalAccess || false}
                onHeadButtonClick={() => setCreateModalActive(true)}>

                <BuildedModulesList error={error} onError={resetValues} loading={loading} modules={modules} />
            </ContentList>

            <ModuleCreateModal active={createModalActive} courseId={course.id} onClose={() => setCreateModalActive(false)} />
        </>

    );
}

function BuildedModulesList(props: {
    error: string,
    onError: () => void,
    loading: boolean,
    modules?: Module[]
}) {
    const { toNext } = useHistoryNavigation();

    if (props.loading) {
        return (<Loader />);
    }

    if (props.error) {
        return (
            <ErrorModal active={Boolean(props.error)} onClose={props.onError} error={props.error} />
        );
    }

    return (
        <>
            {props.modules && props.modules.length > 0 ?
                props.modules.map(module =>
                    <ItemLink
                        title={module.title}
                        checked={module.completed}
                        onClick={() => toNext(Paths.moduleViewPath + '/' + module.id)}
                        iconClassName="icon-module icon-medium-size"
                        className="content-list__item"
                        key={module.id}
                    />) :
                <p>Нет доступных модулей</p>
            }
        </>

    );

}

interface IModuleCreateModalProps {
    courseId: number;
    active: boolean;
    onClose: () => void;

}

function ModuleCreateModal({ courseId, active, onClose }: IModuleCreateModalProps) {
    const [title, setTitle] = useState('');
    const [description, setDescription] = useState('');
    const [titleError, setTitleError] = useState('');

    const { accessToken } = useAppSelector(state => state.authReducer);
    const { createModulesQuery, success, error, loading, resetValues } = useCreateModules();

    useEffect(() => {
    }, [accessToken])

    function onChange(event: React.ChangeEvent) {
        const inputElement = event.target as HTMLInputElement;

        switch (inputElement.name) {
            case 'title':
                setTitle(inputElement.value);
                setTitleError('');
                break;
            case 'description':
                setDescription(inputElement.value);
                break;
        }
    }

    async function createModule() {
        let isValidated = true;

        if (!validate(title)) {
            setTitleError('Название модуля должно быть заполнено')
            isValidated = false;
        }

        if (isValidated && accessToken) {
            await createModulesQuery(courseId, title, accessToken, description);
        }
    }

    function resetDefault() {
        resetValues();
        setTitle('');
        setDescription('');
        setTitleError('');
    }

    function closeModal() {
        resetDefault();
        onClose();
    }

    return (
        <>
            {!loading && !error && !success &&
                <Modal
                    active={active}
                    onClose={closeModal}
                    title="Создание модуля">

                    <ModalContent>
                        <form className="form">
                            <div className="form__inputs">
                                <Input
                                    type={InputType.text}
                                    name="title"
                                    label="Название модуля"
                                    placeholder="Введите название..."
                                    errorMessage={titleError}
                                    value={title}
                                    onChange={onChange}
                                />

                                <Input
                                    type={InputType.rich}
                                    name="description"
                                    label="Описание модуля"
                                    placeholder="Введите описание (необязательно)..."
                                    value={description}
                                    onChange={onChange}
                                />
                            </div>
                        </form>
                    </ModalContent>

                    <ModalFooter>
                        <ModalButton text="Сохранить" onClick={createModule} />
                    </ModalFooter>

                </Modal >
            }

            {loading &&
                <PopupLoader />
            }

            {success &&
                <PopupNotification notificationType={NotificationType.success} onFade={closeModal}>
                    {success}
                </PopupNotification>
            }

            {error &&
                <PopupNotification notificationType={NotificationType.error} onFade={closeModal}>
                    {error}
                </PopupNotification>
            }
        </>
    );
}

export default ModulesList;