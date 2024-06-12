export class Objective {
    id: number;
    title: string;
    text: string;
    formFile: File | null;
    fileUrl?: string;

    constructor(
        title: string,
        text: string,
        formFile: File | null = null
    ) {
        this.id = 0;
        this.title = title;
        this.text = text;
        this.formFile = formFile;
    }
}