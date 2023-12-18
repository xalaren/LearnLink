export class Module {
    id: number;
    title: string;
    description?: string;

    constructor(title: string, description?: string, id: number = 0,) {
        this.id = id;
        this.title = title;
        this.description = description;
    }
}