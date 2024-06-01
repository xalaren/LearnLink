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
