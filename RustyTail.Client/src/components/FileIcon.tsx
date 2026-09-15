interface IFileIconProps {
    fileExtension: string;
}

function FileIcon({ fileExtension }: IFileIconProps) {
    return (
        <div className="file-icon-box">
            <div className="file-icon-box__icon icon-file icon-large-size">
            </div>
            <div className="file-icon-box__extension">
                {fileExtension}
            </div>
        </div>

    );
}

export default FileIcon;