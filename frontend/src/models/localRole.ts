export class LocalRole {
    id: number;
    name: string;
    sign: string;
    isAdmin: boolean;
    viewAccess: boolean;
    editAccess: boolean;
    removeAccess: boolean;
    manageInternalAccess: boolean;
    inviteAccess: boolean;
    kickAccess: boolean;

    constructor(
        name: string,
        sign: string,
        viewAccess: boolean,
        editAccess: boolean,
        removeAccess: boolean,
        manageInternalAccess: boolean,
        inviteAccess: boolean,
        kickAccess: boolean,
        isAdmin: boolean) {

        this.id = 0;
        this.name = name;
        this.sign = sign;
        this.viewAccess = viewAccess;
        this.editAccess = editAccess;
        this.removeAccess = removeAccess;
        this.manageInternalAccess = manageInternalAccess;
        this.inviteAccess = inviteAccess;
        this.kickAccess = kickAccess;
        this.isAdmin = isAdmin;
    }

}