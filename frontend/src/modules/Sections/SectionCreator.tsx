import { useState } from "react";
import { OutlinedInput } from "../../components/Input/OutlinedInput";
import { ContentTypes, InputType } from "../../models/enums";
import ControlNav from "../../components/ControlNav";
import Editor from "../../components/Editor";
import CodeEditor from "../../components/CodeEditor/CodeEditor";
import FileUploader from "../../components/FileUploader/FileUploader";

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

    function onChange(event: React.ChangeEvent) {
        const inputElement = event.target as HTMLInputElement;

        setTitle(inputElement.value);
    }

    function close() {
        setTitle('');
        setText('');
        setFile(null);
        onClose();
    }

    return (
        <>
            {contentType !== ContentTypes.none &&
                <div className="section-create">
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
                            <button className="section-create__button button-white-green icon-check"></button>
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
                </div>
            }
        </>
    );
}

export default SectionCreator;