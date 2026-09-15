export class Role {
    id?: number;
    name: string;
    sign: string;

    constructor(id = 0, name: string, sign: string) {
        this.id = id;
        this.name = name;
        this.sign = sign;
    }
}