export class Content {
    isText: boolean;
    isCodeBlock: boolean;
    isFile: boolean;
    text?: string;
    fileName?: string;
    formFile?: File | null;
    fileUrl?: string;
    fileExtension?: string;
    codeLanguage?: string;

    constructor(
        isText: boolean,
        isCodeBlock: boolean,
        isFile: boolean,
        text?: string,
        codeLanguage?: string,
        formFile?: File | null
    ) {
        this.isText = isText;
        this.isCodeBlock = isCodeBlock;
        this.isFile = isFile;
        this.text = text;
        this.codeLanguage = codeLanguage;
        this.formFile = formFile;
    }
}