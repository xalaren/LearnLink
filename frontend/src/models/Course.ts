export class Course {
    id: number;
    title: string;
    description?: string;
    isPublic: boolean;

    constructor(
        id: number,
        title: string,
        isPublic: boolean,
        description?: string) {
        this.id = id;
        this.title = title;
        this.description = description;
        this.isPublic = isPublic;
    }
}