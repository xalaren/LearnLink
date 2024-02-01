import { useContext } from "react";
import { DropdownContext } from "../contexts/DropdownContext";

interface IDropdownItemProps {
    title: string;
    onClick?: () => void;
    iconPath?: string;
    className?: string;
}

function DropdownItem({ title, onClick, iconPath, className }: IDropdownItemProps) {
    const { deselect } = useContext(DropdownContext);
    return (
        <>
            <li className="dropdown__item"
                onClick={() => {
                    onClick?.();
                    deselect();
                }}>
                {iconPath && <img className={`dropdown__icon`} src={iconPath} alt="icon" />}
                {className && <div className={className}></div>}
                {title}
            </li>
        </>
    );
}

export default DropdownItem;