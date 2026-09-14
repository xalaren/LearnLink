import { LocalRole } from "./localRole";

export interface Participant {
    id: number;
    nickname: string;
    name: string;
    lastname: string;
    avatarUrl?: string;
    localRole: LocalRole;
}