import { useEffect, useState } from "react";
import { OutlinedInput } from "../../components/Input/OutlinedInput";
import { NotificationType } from "../../models/enums";
import ControlNav from "../../components/ControlNav";
import Editor from "../../components/Editor";
import CodeEditor from "../../components/CodeEditor/CodeEditor";
import FileUploader from "../../components/FileUploader/FileUploader";
import { useUpdateSection } from "../../hooks/lessonSectionHook";
import PopupNotification from "../../components/PopupNotification";
import PopupLoader from "../../components/Loader/PopupLoader";
import { Content } from "../../models/content";
import { Section } from "../../models/section";
import { useAppSelector } from "../../hooks/redux";
import { validate } from "../../helpers/validation";
import { ErrorModal } from "../../components/Modal/ErrorModal";
import MiniLoader from "../../components/Loader/MiniLoader";
import DOMPurify from 'dompurify';
import { FileInfo } from "../../models/fileInfo";

interface ISectionEditorProps {
    lessonId: number;
    currentSection: Section;
    onClose: () => void;
}

function SectionEditor({ lessonId, currentSection, onClose }: ISectionEditorProps) {
    const [title, setTitle] = useState('');

    const [text, setText] = useState('');
    const [language, setLanguage] = useState('');
    const [file, setFile] = useState<File | null>(null);

    const [validationError, setValidationError] = useState('');

    const { accessToken } = useAppSelector(state => state.authReducer);
    const { updateSectionQuery, error, loading, success, resetValues } = useUpdateSection();

    useEffect(() => {
        setDefaultValues();
    }, [currentSection]);

    function setDefaultValues() {
        if (!currentSection) return;

        setTitle(currentSection.title);

        if (currentSection.content.isText && currentSection.content.text) {
            setText(currentSection.content.text);
            return;
        }

        if (currentSection.content.isCodeBlock && currentSection.content.text && currentSection.content.codeLanguage) {
            setLanguage(currentSection.content.codeLanguage);
            setText(currentSection.content.text);
            return;
        }
    }


    function onChange(event: React.ChangeEvent) {
        const inputElement = event.target as HTMLInputElement;
        setTitle(inputElement.value);
    }

    function close() {
        setDefaultValues();
        resetValues();
        onClose();
    }

    function resetErrors() {
        setValidationError('');
        resetValues();
    }

    async function updateSection() {

        if (lessonId === 0 || !accessToken) return;

        if (currentSection.content.isText) {
            if (!validate(text)) {
                setValidationError('Текст не был заполнен');
                return;
            }

            const cleanData = DOMPurify.sanitize(text);

            const content = new Content(true, false, false, cleanData);
            const section = { ...currentSection, title, content };

            await updateSectionQuery(lessonId, section, accessToken);

            return;
        }

        if (currentSection.content.isCodeBlock) {
            if (!validate(text)) {
                setValidationError('Код не был заполнен');
                return;
            }

            const content = new Content(false, true, false, text, language);
            const section = { ...currentSection, title, content };

            await updateSectionQuery(lessonId, section, accessToken);
            return;
        }

        const content = new Content(false, false, true, '', '', file);
        const section = { ...currentSection, title, content };

        await updateSectionQuery(lessonId, section, accessToken);
    }

    return (
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
                            <button className="section-create__button button-white-green icon-check" onClick={updateSection}></button>
                            <button className="section-create__button button-white-red icon-cross" onClick={close}></button>
                        </ControlNav>

                    </div>

                    {currentSection.content.isText &&
                        <Editor
                            data={text}
                            setData={setText} />
                    }

                    {currentSection.content.isCodeBlock &&
                        <CodeEditor
                            language={language}
                            setLanguage={setLanguage}
                            text={text}
                            setText={setText}
                        />
                    }

                    {currentSection.content.isFile &&
                        currentSection.content.fileName &&
                        currentSection.content.fileExtension &&
                        currentSection.content.fileUrl &&
                        <FileUploader
                            name="file-upload"
                            file={file}
                            setFile={setFile}
                            uploadedFileInfo={
                                new FileInfo(
                                    currentSection.content.fileName,
                                    currentSection.content.fileExtension,
                                    currentSection.content.fileUrl
                                )}
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
    );
}

export default SectionEditor;