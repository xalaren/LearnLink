import { Module } from "../models/module";

interface IModuleItemProps {
    module: Module;
    onClick: () => void;
}

function ModuleItem({ module, onClick }: IModuleItemProps) {
    return (
        <li className="module-item" onClick={onClick}>
            <div className="medium-little-violet icon-module medium-3"></div>
            <p className="medium-little-violet">{module.title}</p>
        </li>
    );
}

export default ModuleItem;