export class User {
    id?: number;
    nickname: string;
    lastname: string;
    name: string;

    constructor(
        nickname: string,
        lastname: string,
        name: string,
        id?: number) {
        this.id = id;
        this.nickname = nickname;
        this.lastname = lastname;
        this.name = name;
    }
}