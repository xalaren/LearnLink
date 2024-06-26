import { useEffect, useState } from "react";
import Select from "../Select/Select";
import SelectItem from "../Select/SelectItem";
import { codeLanguages } from "../../models/codeLangs";
import { Editor, loader } from "@monaco-editor/react";

interface ICodeEditorProps {
    language: string;
    setLanguage: (language: string) => void;
    text: string;
    setText: (text: string) => void;
}

function CodeEditor({ language, setLanguage, setText, text }: ICodeEditorProps) {
    const [selectActive, setSelectActive] = useState(false);
    const [theme] = useState('atomOneLight');

    useEffect(() => {
        loader.init().then(monaco => {
            monaco.editor.defineTheme('atomOneLight', {
                base: 'vs',
                inherit: true,
                rules: [
                    { foreground: '383a42', token: 'comment' },
                    { foreground: 'a626a4', token: 'keyword' },
                    { foreground: '4078f2', token: 'variable' },
                    { foreground: 'e45649', token: 'number' },
                    { foreground: '50a14f', token: 'string' },
                    { foreground: '986801', token: 'type' },
                    { foreground: '0184bc', token: 'tag' },
                    { foreground: 'c18401', token: 'meta.tag' },
                    { foreground: '4078f2', token: 'attribute.name' },
                    { foreground: '4078f2', token: 'attribute.value' },
                    { foreground: '383a42', token: 'text' }
                ],
                colors: {
                    'editor.background': '#fafafa',
                    'editor.foreground': '#383a42',
                    'editor.lineHighlightBackground': '#fafafa',
                    'editorCursor.foreground': '#526fff',
                    'editorWhitespace.foreground': '#d3d3d3',
                    'editorIndentGuide.background': '#ecebec',
                    'editorIndentGuide.activeBackground': '#e0e0e0',
                    'editor.selectionBackground': '#d6deeb',
                    'editor.lineHighlightBorder': '#eeeeee',
                    'editor.rangeHighlightBackground': '#f0f0f0',
                    'editor.wordHighlightBackground': '#e0e0e0'
                }
            });
        });
    }, []);



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

        <Editor
            height="500px"
            language={language}
            defaultLanguage="javascript"
            value={text}
            onChange={(value) => setText(value || '')}
            options={
                {
                    fontSize: 16,
                    fontFamily: 'Roboto Mono',
                    minimap: {
                        enabled: false
                    },
                    padding: {
                        top: 20,
                        bottom: 20
                    }

                }
            }
            theme={theme}
        />
    </div >);
}

export default CodeEditor;