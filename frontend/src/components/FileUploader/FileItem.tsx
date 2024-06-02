import { FileInfo } from "../../models/fileInfo";
import FileContent from "../Content/FileContent";
import FileIcon from "../FileIcon";

interface IFileItemProps {
    fileInfo: FileInfo;
    onRemove: () => void;
}

function FileItem({ fileInfo }: IFileItemProps) {
    return (
        <div className="file-item">
            <FileContent>
                {[
                    fileInfo
                ]}
            </FileContent>
            <button className="file-item__remove-button icon icon-cross"></button>
        </div>
    );
}

export default FileItem;