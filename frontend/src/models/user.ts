import { Role } from "./role";

export class User {
    id: number;
    nickname: string;
    lastname: string;
    name: string;
    role?: Role;
    avatarFormFile?: any;
    avatarFileName?: string;
    avatarUrl?: string;


    constructor(
        nickname: string,
        lastname: string,
        name: string,
        role?: Role,
        avatarFormFile?: any,
        avatarFileName?: string,
        id = 0) {
        this.id = id;
        this.nickname = nickname;
        this.lastname = lastname;
        this.name = name;
        this.role = role;
        this.avatarFormFile = avatarFormFile;
        this.avatarFileName = avatarFileName;
    }
}