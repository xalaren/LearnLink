import { FileInfo } from "../../models/fileInfo";
import FileContent from "../Content/FileContent";

interface IFileItemProps {
    fileInfo: FileInfo;
    onRemove: () => void;
}

function FileItem({ fileInfo, onRemove }: IFileItemProps) {
    return (
        <div className="file-item">
            <FileContent>
                {[
                    fileInfo
                ]}
            </FileContent>
            <button className="file-item__remove-button icon icon-cross" onClick={onRemove}></button>
        </div>
    );
}

export default FileItem;