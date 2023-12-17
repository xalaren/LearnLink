export class Module {
    id: number;
    title: string;
    description?: string;

    constructor(title: string, id: number = 0, description?: string) {
        this.id = id;
        this.title = title;
        this.description = description;
    }
}