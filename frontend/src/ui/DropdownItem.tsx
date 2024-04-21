import { useContext } from "react";
import { DropdownContext } from "../contexts/DropdownContext";

interface IDropdownItemProps {
    title: string;
    onClick?: () => void;
    className?: string;
}

function DropdownItem({ title, onClick, className }: IDropdownItemProps) {
    const { deselect } = useContext(DropdownContext);
    return (
        <div className="dropdown__item"
            onClick={() => {
                onClick?.();
                deselect();
            }}>
            {className && <span className={className}></span>}
            {title}
        </div>
    );
}

export default DropdownItem;