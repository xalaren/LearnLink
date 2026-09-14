import {LocalRole} from "./localRole.ts";

export class RoleItemModel {
    localRole: LocalRole;
    onEdit: () => void;
    onDelete: () => void;

    constructor(localRole: LocalRole, onEdit: () => void, onDelete: () => void) {
        this.localRole = localRole;
        this.onEdit = onEdit;
        this.onDelete = onDelete;
    }

}