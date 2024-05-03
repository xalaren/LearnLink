import { LocalRole } from "./localRole";

export class Course {
    id: number;
    title: string;
    description?: string;
    subscribersCount: number;
    isPublic: boolean;
    isUnavailable: boolean;
    creationDate: string = '';
    completionProgress?: number;
    completed?: boolean;
    localRole?: LocalRole;

    constructor(
        id: number,
        title: string,
        isPublic: boolean,
        isUnavailable: boolean,
        description?: string,
        subscribersCount: number = 0
    ) {
        this.id = id;
        this.title = title;
        this.isPublic = isPublic;
        this.isUnavailable = isUnavailable;
        this.description = description;
        this.subscribersCount = subscribersCount;
    }
}