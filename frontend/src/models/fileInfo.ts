export class FileInfo {
    fileName: string;
    fileExtension: string;
    url?: string;

    constructor(
        fileName: string,
        fileExtension: string,
        url: string = ''
    ) {
        this.fileName = fileName;
        this.fileExtension = fileExtension;
        this.url = url;
    }
}