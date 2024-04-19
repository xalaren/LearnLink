export class Course {
    id: number;
    title: string;
    description?: string;
    creationDate: Date;
    subscribersCount: number;
    isPublic: boolean;
    isUnavailable: boolean;

    constructor(
        id: number,
        title: string,
        isPublic: boolean,
        isUnavailable: boolean,
        description?: string,
        subscribersCount: number = 0,
        creationDate: Date = new Date()
    ) {
        this.id = id;
        this.title = title;
        this.isPublic = isPublic;
        this.isUnavailable = isUnavailable;
        this.description = description;
        this.subscribersCount = subscribersCount;
        this.creationDate = creationDate;
    }
}