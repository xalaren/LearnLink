export class Course {
    id: number;
    description?: string;
    title: string;
    isPublic: boolean;

    constructor(id: number, title: string, isPublic: boolean, description?: string) {
        this.id = id;
        this.title = title;
        this.isPublic = isPublic;
        this.description = description;
    }
}