import { Module } from "../models/module";
import ModuleIcon from "./ModuleIcon";

interface IModuleItemProps {
    module: Module;
    onClick: () => void;
}

function ModuleItem({ module, onClick }: IModuleItemProps) {
    return (
        <li className="module-item" onClick={onClick}>
            <ModuleIcon />
            <p className="medium-little-violet">{module.title}</p>
        </li>
    );
}

export default ModuleItem;