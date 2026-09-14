import { Content } from "./content";

export class Section {
    id: number;
    order: number = 0;
    content: Content;
    title: string;

    constructor(
        content: Content,
        title: string = "",
        id = 0,
    ) {
        this.id = id;
        this.content = content;
        this.title = title;
    }
}

export interface SectionCodeContent {
    id: number;
    order: number;
    title?: string;
    code: string;
    codeLanguage: string;
}

export interface SectionFileContent {
    id: number;
    order: number;
    title?: string;
    formFile: File;
}

export interface SectionTextContent {
    id: number;
    order: number;
    title?: string;
    text: string;
}


