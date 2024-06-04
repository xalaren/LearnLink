import { useEffect, useRef, useState } from "react";
import Select from "../Select/Select";
import SelectItem from "../Select/SelectItem";
import { codeLanguages } from "../../models/codeLangs";

interface ICodeEditorProps {
    language: string;
    setLanguage: (language: string) => void;
    text: string;
    setText: (text: string) => void;
}

function CodeEditor({ language, setLanguage, setText, text }: ICodeEditorProps) {
    const [selectActive, setSelectActive] = useState(false);
    const [lineNumbers, setLineNumbers] = useState(['1']);
    const textareaRef = useRef<HTMLTextAreaElement>(null);

    function onChange(event: React.ChangeEvent) {
        const inputElement = event.target as HTMLInputElement;

        setText(inputElement.value);
        refreshLineNumbers();
    }

    const handleKeyDown = (event: React.KeyboardEvent) => {
        if (!textareaRef.current) return;

        if (event.key === 'Tab') {
            event.preventDefault();
            const { selectionStart, selectionEnd } = event.target as HTMLTextAreaElement;
            const value = text;
            if (event.shiftKey) {
                // Handle Shift + Tab
                const beforeTab = value.lastIndexOf('\t', selectionStart - 1);
                if (beforeTab !== -1 && beforeTab === selectionStart - 1) {
                    const newValue = value.substring(0, selectionStart - 1) + value.substring(selectionStart);
                    setText(newValue);
                    textareaRef.current.selectionStart = textareaRef.current.selectionEnd = selectionStart - 1;
                }
            } else {
                // Handle Tab
                const newValue = value.substring(0, selectionStart) + '\t' + value.substring(selectionEnd);
                setText(newValue);
                textareaRef.current.selectionStart = textareaRef.current.selectionEnd = selectionStart + 1;
            }
        }
    };

    function refreshLineNumbers() {
        const numberOfLines = text.split('\n').length;
        const newLineNumbers = Array.from({ length: numberOfLines }, (_, i) => (i + 1).toString());
        setLineNumbers(newLineNumbers);
    }


    return (<div className='code-block'>
        <div className="code-block__title">
            <Select
                active={selectActive}
                onDeselect={() => setSelectActive(false)}
                toggle={() => setSelectActive(prev => !prev)}
                defaultTitle="Выберите язык..."
                selectedTitle={language}>
                {codeLanguages.map(lang =>
                    <SelectItem
                        key={codeLanguages.indexOf(lang)}
                        title={lang}
                        onSelect={() => {
                            setLanguage(lang);
                            setSelectActive(false);
                        }}
                    />)
                }
            </Select>
        </div>
        <div className="code-block__code">
            <div className="code-block__line-numbers">
                {lineNumbers.map((line, index) =>
                    <p key={index}>
                        {line}
                    </p>)}
            </div>
            <textarea
                ref={textareaRef}
                className="textarea"
                value={text}
                onChange={onChange}
                onKeyDown={handleKeyDown}
                onBlur={refreshLineNumbers}
            >
            </textarea>
        </div>

    </div >);
}

export default CodeEditor;