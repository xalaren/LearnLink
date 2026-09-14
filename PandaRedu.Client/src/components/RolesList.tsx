import {RoleItemModel} from "../models/roleItemModel.ts";

interface IRolesListProps {
    children: RoleItemModel[];
}

function RolesList({ children }: IRolesListProps) {
    return (
        <div className="roles-list">
            {children.map(item =>
                <RoleItem
                    name={item.localRole.name}
                    key={item.localRole.id}
                    onEdit={item.onEdit}
                    onDelete={item.onDelete}/>)
            }
        </div>
    )
}

interface IRoleItemProps {
    name: string;
    onEdit: () => void;
    onDelete: () => void;
}
function RoleItem({name, onEdit, onDelete}: IRoleItemProps) {
    return (
        <div className="roles-list__item role-item">
            <div className="role-item__info">
                <div className="role-item__icon icon icon-medium-size icon-lock"></div>
                <div className="role-item__name">{name}</div>
            </div>
            <div className="role-item__properties">
                <button
                    className="role-item__button button-violet-light-rounded icon icon-medium-size icon-pen"
                    onClick={onEdit}></button>
                <button
                    className="role-item__button button-red-light-rounded icon icon-medium-size icon-cross"
                    onClick={onDelete}></button>
            </div>
        </div>
    );
}

export default RolesList;