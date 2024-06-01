import { FileInfo } from "../../models/fileInfo";
import FileIcon from "../FileIcon";

interface IFileContentProps {
    children: FileInfo[];
}

function FileContent({ children }: IFileContentProps) {
    function redirect(url: string) {
        window.open(url, '_blank')?.focus();
    }

    function download(fileName: string, url: string) {
        const element = document.createElement('a');
        element.setAttribute('href', url);
        element.setAttribute('download', fileName);
        element.style.display = 'none';
        document.body.appendChild(element);
        element.click();
        document.body.removeChild(element);
    }

    return (
        <div className="lesson-section__files files-list">
            {children.map(file =>
                <button className="button button-gray-violet" onClick={() => download(file.fileName, file.url)}>
                    <FileIcon fileExtension={file.fileExtension} />
                    {file.fileName}
                </button>
            )
            }
        </div>
    );
}

export default FileContent;