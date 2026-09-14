export class Module {
    id: number;
    title: string;
    description?: string;
    completed: boolean = false;
    completionProgress?: number;

    constructor(title: string, description?: string, id: number = 0) {
        this.id = id;
        this.title = title;
        this.description = description;
    }
}