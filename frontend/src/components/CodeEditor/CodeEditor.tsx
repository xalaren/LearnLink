import { useEffect, useState } from "react";
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

    function onChange(event: React.ChangeEvent) {
        const inputElement = event.target as HTMLInputElement;

        setText(inputElement.value);

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
                    <p
                        key={index}
                    >
                        {line}
                    </p>)}
            </div>
            <textarea
                className="textarea"
                value={text}
                onChange={onChange}
            >
            </textarea>
        </div>

    </div>);
}

export default CodeEditor;