export class FileInfo {
    name: string;
    extension: string;
    url: string;

    constructor(
        fileName: string,
        fileExtension: string,
        url: string = ''
    ) {
        this.name = fileName;
        this.extension = fileExtension;
        this.url = url;
    }
}