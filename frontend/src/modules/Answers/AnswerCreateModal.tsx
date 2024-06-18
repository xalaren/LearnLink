import { useEffect, useState } from "react";
import PopupLoader from "../../components/Loader/PopupLoader";
import { Modal } from "../../components/Modal/Modal";
import ModalButton from "../../components/Modal/ModalButton";
import ModalContent from "../../components/Modal/ModalContent";
import ModalFooter from "../../components/Modal/ModalFooter";
import PopupNotification from "../../components/PopupNotification";
import { validate } from "../../helpers/validation";
import { NotificationType } from "../../models/enums";
import { useAppSelector } from "../../hooks/redux";
import Editor from "../../components/Editor";
import FileUploader from "../../components/FileUploader/FileUploader";
import CollapseableBlock from "../../components/CollapseableBlock";
import { FileInfo } from "../../models/fileInfo";
import { useAnswerQueries } from "../../hooks/answerHooks";
import { Answer } from "../../models/answer";

interface IAnswerCreateModalProps {
    lessonId: number;
    objectiveId: number;
    active: boolean;
    onClose: () => void;
}

function AnswerCreateModal({ lessonId, objectiveId, active, onClose }: IAnswerCreateModalProps) {
    const [text, setText] = useState('');

    const [fileUploaderCollapsed, setFileUploaderCollapsed] = useState(true);

    const [file, setFile] = useState<File | null>(null);
    const [uploadedFileInfo, setUploadedFileInfo] = useState<FileInfo | null>(null);

    const { user } = useAppSelector(state => state.userReducer);
    const { accessToken } = useAppSelector(state => state.authReducer);
    const { createQuery, success, error, loading, resetValues } = useAnswerQueries();

    useEffect(() => {
    }, [accessToken, user]);

    async function createAnswer() {
        if (!user || !accessToken) return;

        if (!validate(text) && !file) {
            closeModal();
            return;
        }

        const answer: Answer = {
            id: 0,
            objectiveId: objectiveId,
            userId: user.id,
            userDetails: {
                id: user.id,
                lastname: user.lastname,
                name: user.name,
                nickname: user.nickname,
                avatarUrl: user.avatarUrl
            },
            text: text,
            uploadDate: Date.now.toString(),
            formFile: file
        };

        await createQuery(lessonId, answer, accessToken);
    }

    function resetDefault() {
        resetValues();
        setText('');
        setFile(null);
        setFileUploaderCollapsed(true);
        setUploadedFileInfo(null);
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
                    title="Добавить ответ на задание:"
                    contentClassName="large-modal"
                >

                    <ModalContent>
                        <form className="form">
                            <div className="form__inputs">
                                <div className="form-input">
                                    <p className={`form-input__label`}>Текст ответа:</p>
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
                                        setUploadedFileInfo={setUploadedFileInfo}
                                    />
                                </CollapseableBlock>

                            </div>
                        </form>
                    </ModalContent>

                    <ModalFooter>
                        <ModalButton text="Сохранить" onClick={createAnswer} />
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

export default AnswerCreateModal;