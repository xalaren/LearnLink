export class Course {
    id: number;
    description?: string;
    title: string;
    isPublic: boolean;
    creatorsCount?: number;
    subscribersCount?: number;

    constructor(id: number, title: string, isPublic: boolean, description?: string, creatorsCount?: number, subscribersCount?: number) {
        this.id = id;
        this.title = title;
        this.isPublic = isPublic;
        this.description = description;
        this.creatorsCount = creatorsCount;
        this.subscribersCount = subscribersCount;
    }
}