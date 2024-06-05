import { useState } from "react";
import { OutlinedInput } from "../../components/Input/OutlinedInput";
import { ContentTypes, NotificationType } from "../../models/enums";
import ControlNav from "../../components/ControlNav";
import Editor from "../../components/Editor";
import CodeEditor from "../../components/CodeEditor/CodeEditor";
import FileUploader from "../../components/FileUploader/FileUploader";
import { useCreateSection } from "../../hooks/sectionHooks";
import PopupNotification from "../../components/PopupNotification";
import PopupLoader from "../../components/Loader/PopupLoader";
import { Content } from "../../models/content";
import { Section } from "../../models/section";
import { useAppSelector } from "../../hooks/redux";
import { validate } from "../../helpers/validation";
import { ErrorModal } from "../../components/Modal/ErrorModal";
import MiniLoader from "../../components/Loader/MiniLoader";
import DOMPurify from 'dompurify';

interface ISectionCreatorProps {
    lessonId: number;
    contentType: ContentTypes;
    onClose: () => void;
}

function SectionCreator({ lessonId, contentType, onClose }: ISectionCreatorProps) {
    const [title, setTitle] = useState('');

    const [text, setText] = useState('');
    const [language, setLanguage] = useState('');
    const [file, setFile] = useState<File | null>(null);

    const [validationError, setValidationError] = useState('');

    const { accessToken } = useAppSelector(state => state.authReducer);
    const { createSectionQuery, error, success, loading, resetValues } = useCreateSection();


    function onChange(event: React.ChangeEvent) {
        const inputElement = event.target as HTMLInputElement;
        setTitle(inputElement.value);
    }

    function close() {
        setTitle('');
        setText('');
        setFile(null);
        resetValues();
        onClose();
    }

    function resetErrors() {
        setValidationError('');
        resetValues();
    }

    async function createSection() {
        if (contentType === ContentTypes.none) return;

        if (lessonId === 0 || !accessToken) return;

        if (contentType === ContentTypes.text) {
            if (!validate(text)) {
                setValidationError('Текст не был заполнен');
                return;
            }

            const cleanData = DOMPurify.sanitize(text);

            const content = new Content(true, false, false, cleanData);
            const section = new Section(content, lessonId, title);

            await createSectionQuery(section, accessToken);
            return;
        }

        if (contentType === ContentTypes.code) {
            if (!validate(text)) {
                setValidationError('Код не был заполнен');
                return;
            }

            const content = new Content(false, true, false, text, language);
            const section = new Section(content, lessonId, title);

            await createSectionQuery(section, accessToken);
            return;
        }

        if (!file) {
            setValidationError("Файл не был загружен");
        }

        const content = new Content(false, false, true, '', '', file);
        const section = new Section(content, lessonId, title);
        await createSectionQuery(section, accessToken);
    }

    return (
        <>
            {contentType !== ContentTypes.none &&
                <div className="section-create">
                    {!success && !error ?
                        <>
                            <div className="line-distributed-container">
                                <OutlinedInput
                                    className="section-create__input"
                                    name="title"
                                    onChange={onChange}
                                    required={false}
                                    value={title}
                                    placeholder="Название раздела..."
                                />

                                <ControlNav>
                                    <button className="section-create__button button-white-green icon-check" onClick={createSection}></button>
                                    <button className="section-create__button button-white-red icon-cross" onClick={close}></button>
                                </ControlNav>

                            </div>

                            {contentType === ContentTypes.text &&
                                <Editor
                                    data={text}
                                    setData={setText} />
                            }

                            {contentType === ContentTypes.code &&
                                <CodeEditor
                                    language={language}
                                    setLanguage={setLanguage}
                                    text={text}
                                    setText={setText}
                                />
                            }

                            {contentType === ContentTypes.file &&
                                <FileUploader
                                    name="file-upload"
                                    file={file}
                                    setFile={setFile}
                                />
                            }
                        </> :
                        <MiniLoader />
                    }



                    {validationError &&
                        <ErrorModal
                            active={Boolean(validationError)}
                            error={validationError}
                            onClose={resetErrors}
                        />
                    }

                    {error &&
                        <PopupNotification notificationType={NotificationType.error} onFade={resetValues}>
                            {error}
                        </PopupNotification>
                    }

                    {success &&
                        <PopupNotification notificationType={NotificationType.success} onFade={close}>
                            {success}
                        </PopupNotification>
                    }

                    {!error && !success && loading &&
                        <PopupLoader />
                    }
                </div>
            }
        </>
    );
}

export default SectionCreator;