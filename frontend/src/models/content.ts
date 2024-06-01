export class Content {
    isText: boolean;
    isCodeBlock: boolean;
    isFile: boolean;
    text?: string;
    formFile?: File;
    fileName?: string;
    fileExtension?: string;
    codeLanguage?: string;
    fileUrl?: string;

    constructor(
        isText: boolean,
        isCodeBlock: boolean,
        isFile: boolean,
        text?: string,
        codeLanguage?: string,
        formFile?: File
    ) {
        this.isText = isText;
        this.isCodeBlock = isCodeBlock;
        this.isFile = isFile;
        this.text = text;
        this.codeLanguage = codeLanguage;
        this.formFile = formFile;
    }
}