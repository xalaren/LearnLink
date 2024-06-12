import { useEffect, useState } from "react";
import { Input } from "../../components/Input/Input";
import PopupLoader from "../../components/Loader/PopupLoader";
import { Modal } from "../../components/Modal/Modal";
import ModalButton from "../../components/Modal/ModalButton";
import ModalContent from "../../components/Modal/ModalContent";
import ModalFooter from "../../components/Modal/ModalFooter";
import PopupNotification from "../../components/PopupNotification";
import { validate } from "../../helpers/validation";
import { InputType, NotificationType } from "../../models/enums";
import { useAppSelector } from "../../hooks/redux";
import { useObjectiveQueries } from "../../hooks/objectiveHooks";
import Editor from "../../components/Editor";
import FileUploader from "../../components/FileUploader/FileUploader";
import CollapseableBlock from "../../components/CollapseableBlock";
import { Objective } from "../../models/objective";

interface IObjectiveCreateModalProps {
    lessonId: number;
    active: boolean;
    onClose: () => void;

}

function ObjectiveCreateModal({ lessonId, active, onClose }: IObjectiveCreateModalProps) {
    const [title, setTitle] = useState('');
    const [titleError, setTitleError] = useState('');

    const [text, setText] = useState('');
    const [textError, setTextError] = useState('');

    const [fileUploaderCollapsed, setFileUploaderCollapsed] = useState(true);

    const [file, setFile] = useState<File | null>(null);

    const { accessToken } = useAppSelector(state => state.authReducer);
    const { createQuery, success, error, loading, resetValues } = useObjectiveQueries();

    useEffect(() => {
    }, [accessToken]);

    async function createObjective() {
        let isValidated = true;

        if (!validate(title)) {
            setTitleError('Название модуля должно быть заполнено')
            isValidated = false;
        }

        if (!validate(text)) {
            setTextError('Текст задания должен быть заполнен');
        }

        if (isValidated && accessToken) {
            const objective = new Objective(title, text, file);
            await createQuery(lessonId, objective, accessToken);
        }
    }

    function resetDefault() {
        resetValues();
        setTitle('');
        setText('');
        setTitleError('');
        setTextError('');
        setFile(null);
        setFileUploaderCollapsed(true);
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
                    title="Создание задания">

                    <ModalContent>
                        <form className="form">
                            <div className="form__inputs">
                                <Input
                                    type={InputType.text}
                                    name="title"
                                    label="Название задания"
                                    placeholder="Введите название..."
                                    errorMessage={titleError}
                                    required={true}
                                    value={title}
                                    onChange={(event) => setTitle((event.target as HTMLInputElement).value)}
                                />
                                <div className="form-input">
                                    {!textError &&
                                        <p className={`form-input__label form-input__label-required`}>Текст задания:</p>
                                    }
                                    {textError &&
                                        <p className="form-input__error required">{textError}</p>
                                    }
                                    <Editor
                                        data={text}
                                        setData={setText}
                                    />
                                </div>

                                <CollapseableBlock
                                    title="Загрузка файла"
                                    collapsed={fileUploaderCollapsed}
                                    collapseChange={() => {
                                        setFileUploaderCollapsed(prev => !prev)
                                    }}>
                                    <FileUploader
                                        name="file"
                                        file={file}
                                        setFile={setFile} />
                                </CollapseableBlock>

                            </div>
                        </form>
                    </ModalContent>

                    <ModalFooter>
                        <ModalButton text="Сохранить" onClick={createObjective} />
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

export default ObjectiveCreateModal;