import { User } from "./user";

export class InviteItem {
    user: User;
    onInvite: () => void;

    constructor(user: User, onInvite: () => void) {
        this.user = user;
        this.onInvite = onInvite;
    }
}