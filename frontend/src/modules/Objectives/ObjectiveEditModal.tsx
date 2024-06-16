import { useContext, useEffect, useState } from "react";
import { CourseContext } from "../../contexts/CourseContext";
import { ModuleContext } from "../../contexts/ModuleContext";
import { useAppSelector } from "../../hooks/redux";
import { validate } from "../../helpers/validation";
import { Modal } from "../../components/Modal/Modal";
import ModalContent from "../../components/Modal/ModalContent";
import { Input } from "../../components/Input/Input";
import { InputType, NotificationType } from "../../models/enums";
import ModalFooter from "../../components/Modal/ModalFooter";
import ModalButton from "../../components/Modal/ModalButton";
import PopupLoader from "../../components/Loader/PopupLoader";
import PopupNotification from "../../components/PopupNotification";
import { ObjectiveContext } from "../../contexts/ObjectiveContext";
import { useObjectiveQueries } from "../../hooks/objectiveHooks";
import { FileInfo } from "../../models/fileInfo";
import CollapseableBlock from "../../components/CollapseableBlock";
import FileUploader from "../../components/FileUploader/FileUploader";
import Editor from "../../components/Editor";
import { Objective } from "../../models/objective";

interface IObjectiveEditModalProps {
    active: boolean;
    onClose: () => void;
}

function ModuleEditModal({ active, onClose }: IObjectiveEditModalProps) {
    const { objective, fetchObjective } = useContext(ObjectiveContext);

    const [title, setTitle] = useState('');
    const [titleError, setTitleError] = useState('');

    const [text, setText] = useState('');
    const [textError, setTextError] = useState('');

    const [fileUploaderCollapsed, setFileUploaderCollapsed] = useState(true);

    const [file, setFile] = useState<File | null>(null);
    const [uploadedFileInfo, setUpdloadedFileInfo] = useState<FileInfo | null>(null);

    const { accessToken } = useAppSelector(state => state.authReducer);
    const { updateQuery, success, error, loading, resetValues } = useObjectiveQueries();

    useEffect(() => {
        if (objective) {
            setTitle(objective.title);
            setText(objective.text);

            if (objective.fileName && objective.fileExtension && objective.fileUrl) {
                setUpdloadedFileInfo(new FileInfo(objective.fileName, objective.fileExtension, objective.fileUrl));
            }
        }
    }, [objective, accessToken]);

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
        fetchObjective();
        resetDefault();
        onClose();
    }

    async function updateObjective() {
        let isValidated = true;

        if (!validate(title)) {
            setTitleError('Название модуля должно быть заполнено')
            isValidated = false;
        }

        if (!validate(text)) {
            setTextError('Текст задания должен быть заполнен');
        }

        if (isValidated && objective && accessToken) {
            const newObjective: Objective = { ...objective, title: title, text: text, formFile: file };

            await updateQuery(newObjective, uploadedFileInfo == null, accessToken);
        }
    }

    return (
        <>
            {!loading && !error && !success &&
                <Modal
                    active={active}
                    onClose={closeModal}
                    title="Редактирование задания"
                    contentClassName="large-modal"
                >

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
                                        setFile={setFile}
                                        uploadedFileInfo={uploadedFileInfo}
                                        setUploadedFileInfo={setUpdloadedFileInfo}
                                    />
                                </CollapseableBlock>

                            </div>
                        </form>
                    </ModalContent>

                    <ModalFooter>
                        <ModalButton text="Сохранить" onClick={updateObjective} />
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

export default ModuleEditModal;