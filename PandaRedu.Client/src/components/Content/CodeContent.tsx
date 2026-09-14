import SyntaxHighlighter from 'react-syntax-highlighter';
import { atomOneLight } from 'react-syntax-highlighter/dist/esm/styles/hljs';

interface ICodeContentProps {
    language: string;
    children: string;
}

function CodeContent({ language, children }: ICodeContentProps) {

    return (
        <div className='code-block'>
            <div className="code-block__title">{language}</div>
            <div className="code-block__text">
                <SyntaxHighlighter language={language} style={atomOneLight}>
                    {children}
                </SyntaxHighlighter>
            </div>
        </div>
    );
}

export default CodeContent;