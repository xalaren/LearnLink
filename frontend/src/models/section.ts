import { Content } from "./content";

export class Section {
    id: number;
    order: number = 0;
    content: Content;
    lessonId: number;
    title: string;

    constructor(
        content: Content,
        lessonId: number,
        title: string = "",
        id = 0,
    ) {
        this.id = id;
        this.content = content;
        this.lessonId = lessonId;
        this.title = title;
    }
}

export interface SectionCodeContent {
    id: number;
    order: number;
    lessonId: number;
    title?: string;
    code: string;
    codeLanguage: string;
}

export interface SectionFileContent {
    id: number;
    order: number;
    lessonId: number;
    title?: string;
    formFile: File;
}

export interface SectionTextContent {
    id: number;
    order: number;
    lessonId: number;
    title?: string;
    text: string;
}


