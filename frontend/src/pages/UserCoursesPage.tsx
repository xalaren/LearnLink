import { MainContainer } from "../components/MainContainer";
import { useHistoryNavigation } from "../hooks/historyNavigation";
import { useAppSelector } from "../hooks/redux";
import { useEffect, useState } from "react";
import { Paths } from "../models/paths";
import { useParams } from "react-router-dom";
import SelectionPanel from "../components/Selection/SelectionPanel";
import SelectionItem from "../components/Selection/SelectionItem";
import { InputType, NotificationType, ViewTypes } from "../models/enums";
import ControlNav from "../components/ControlNav";
import UserCoursesModule from "../modules/UserCoursesModule";
import { Modal } from "../components/Modal/Modal";
import ModalContent from "../components/Modal/ModalContent";
import ModalFooter from "../components/Modal/ModalFooter";
import ModalButton from "../components/Modal/ModalButton";
import { Input } from "../components/Input";
import { validate } from "../helpers/validation";
import Checkbox from "../components/Checkbox";
import { useCreateCourse } from "../hooks/courseHooks";
import PopupLoader from "../components/Loader/PopupLoader";
import PopupNotification from "../components/PopupNotification";

function UserCoursesPage() {
    const { toNext } = useHistoryNavigation();
    const { isAuthenticated } = useAppSelector(state => state.authReducer);
    const param = useParams<'type'>();
    const [createModalActive, setCreateModalActive] = useState(false);


    useEffect(() => {
        if (!isAuthenticated) toNext(Paths.homePath);

    }, [isAuthenticated, toNext])


    return (
        <MainContainer>
            <div className="line-distributed-container">
                <h3>Мои курсы</h3>
                <ControlNav>
                    <button className="control-nav__add-button button-gray icon-plus" onClick={() => setCreateModalActive(true)}></button>
                </ControlNav>
            </div>

            <SelectionPanel>
                <SelectionItem
                    index={1}
                    className="selection-panel__selection-item"
                    title="Созданные"
                    active={param.type === ViewTypes.created}
                    onClick={() => toNext(`${Paths.userCoursesPath}/${ViewTypes.created}/page/1`)}
                />

                <SelectionItem
                    index={2}
                    className="selection-panel__selection-item"
                    title="Подписки"
                    active={param.type === ViewTypes.subscribed}
                    onClick={() => toNext(`${Paths.userCoursesPath}/${ViewTypes.subscribed}/page/1`)}
                />

                <SelectionItem
                    index={3}
                    className="selection-panel__selection-item"
                    title="Скрытые"
                    active={param.type === ViewTypes.unavailable}
                    onClick={() => toNext(`${Paths.userCoursesPath}/${ViewTypes.unavailable}/page/1`)}
                />
            </SelectionPanel>

            <UserCoursesModule type={param.type} shouldUpdate={createModalActive} />

            <CourseCreateModal active={createModalActive} onClose={() => setCreateModalActive(false)} />
        </MainContainer>
    );
}

interface ICourseCreateModalProps {
    active: boolean;
    onClose: () => void;
}

function CourseCreateModal({ active, onClose }: ICourseCreateModalProps) {
    const [title, setTitle] = useState('');
    const [description, setDescription] = useState('');
    const [isPublic, setPublic] = useState(false);
    const [titleError, setTitleError] = useState('');

    const { accessToken } = useAppSelector(state => state.authReducer);
    const { user } = useAppSelector(state => state.userReducer);
    const { createCourseQuery, loading, error, success, resetValues } = useCreateCourse();

    useEffect(() => {
    }, [user])

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

    async function createCourse() {
        let isValidated = true;

        if (!validate(title)) {
            setTitleError('Название курса должно быть заполнено')
            isValidated = false;
        }

        if (isValidated && user && accessToken) {
            await createCourseQuery(title, isPublic, user.id, accessToken, description);
        }
    }

    function resetDefault() {
        resetValues();
        setTitle('');
        setDescription('');
        setTitleError('');
        setPublic(false);
    }

    function closeModal() {
        resetDefault();
        onClose();
    }

    return (
        <>

            <Modal
                active={active}
                onClose={closeModal}
                title="Создание курса">

                <ModalContent>
                    <form className="form">
                        <div className="form__inputs">
                            <Input
                                type={InputType.text}
                                name="title"
                                label="Название курса"
                                placeholder="Введите название..."
                                errorMessage={titleError}
                                value={title}
                                onChange={onChange}
                            />

                            <Input
                                type={InputType.rich}
                                name="description"
                                label="Описание курса"
                                placeholder="Введите описание (необязательно)..."
                                value={description}
                                onChange={onChange}
                            />

                            <Checkbox
                                isChecked={isPublic}
                                checkedChanger={() => setPublic(prev => !prev)}
                                label="Публикация в общий доступ"
                            />
                        </div>
                    </form>
                </ModalContent>

                <ModalFooter>
                    <ModalButton text="Сохранить" onClick={createCourse} />
                </ModalFooter>

            </Modal >

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

export default UserCoursesPage;