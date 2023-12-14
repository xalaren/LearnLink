export class Course {
    id: number;
    description?: string;
    title: string;
    isPublic: boolean;
    subscribersCount?: number;

    constructor(id: number, title: string, isPublic: boolean, description?: string, subscribersCount?: number) {
        this.id = id;
        this.title = title;
        this.isPublic = isPublic;
        this.description = description;
        this.subscribersCount = subscribersCount;
    }
}