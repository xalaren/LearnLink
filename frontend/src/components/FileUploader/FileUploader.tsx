import { useEffect, useState } from "react";
import { FileInfo } from "../../models/fileInfo";
import FileItem from "./FileItem";

interface IFileUploaderProps {
    name: string;
    file?: File | null;
    setFile: (file: File) => void;
    uploadedFileInfo?: FileInfo;
}

function FileUploader({ name, file, setFile, uploadedFileInfo }: IFileUploaderProps) {
    const [drag, setDrag] = useState(false);
    const [fileInfo, setFileInfo] = useState<FileInfo | null>(null);

    useEffect(() => {
        if (file) {
            const fileUrl = URL.createObjectURL(file);
            const fileExt = file.name.split('.').pop() || 'file';
            setFileInfo(new FileInfo(file.name, fileExt, fileUrl))
        }
        else if (uploadedFileInfo) {
            setFileInfo(uploadedFileInfo);
        }
        else {
            setFileInfo(null);
        }
    }, [file]);


    function dragStartHandler(event: React.DragEvent) {
        event.preventDefault();
        setDrag(true);
    }

    function dragLeaveHandler(event: React.DragEvent) {
        event.preventDefault();
        setDrag(false);
    }

    function dropHandler(event: React.DragEvent) {
        event.preventDefault();
        const files = [...event.dataTransfer.files];

        if (files) {
            setFile(files[0]);
        }
        setDrag(false);
    }

    function onChange(event: React.ChangeEvent) {
        setDrag(false);
        const inputElement = event.target as HTMLInputElement;

        if (inputElement.files) {
            setFile(inputElement.files[0]);
        }
    }

    function onRemove() {
        setFileInfo(null);
    }

    return (
        <div className="file-uploader">
            {fileInfo ?
                <>
                    <p>Загруженные файлы:</p>
                    <FileItem
                        fileInfo={fileInfo}
                        onRemove={onRemove}
                    />
                </>
                :
                <label
                    className={`drag-n-drop ${drag && 'drag-n-drop-dragging'}`}
                    htmlFor="file-upload"
                    onDragStart={dragStartHandler}
                    onDragLeave={dragLeaveHandler}
                    onDragOver={dragStartHandler}
                    onDrop={dropHandler}
                >

                    <div className="drag-n-drop__content">
                        <div className="drag-n-drop__icon">
                            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="size-6">
                                <path strokeLinecap="round" strokeLinejoin="round" d="M12 16.5V9.75m0 0 3 3m-3-3-3 3M6.75 19.5a4.5 4.5 0 0 1-1.41-8.775 5.25 5.25 0 0 1 10.233-2.33 3 3 0 0 1 3.758 3.848A3.752 3.752 0 0 1 18 19.5H6.75Z" />
                            </svg>
                        </div>
                        <div className="drag-n-drop__text">{
                            drag ?
                                <p>Отпустите, чтобы загрузить файл...</p> :
                                <p>Перетащите файл, чтобы его загрузить...</p>
                        }</div>
                    </div>

                    <input
                        type="file"
                        className="drag-n-drop__input"
                        name={name}
                        onChange={onChange}
                        id="file-upload"
                    />
                </label>
            }
        </div>
    );
}

export default FileUploader;