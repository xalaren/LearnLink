import { CSSProperties, useContext } from "react";
import { Dropdown } from "../ui/Dropdown";
import DropdownHead from "../ui/DropdownHead";
import { DropdownContext } from "../contexts/DropdownContext";

interface IDropdownButtonProps {
    title: string,
    children: React.ReactNode,
    itemStyles?: CSSProperties;
}

export function DropdownButton({ title, children, itemStyles }: IDropdownButtonProps) {
    const { active, toggle, deselect } = useContext(DropdownContext);
    return (
        <Dropdown active={active} onDeselect={deselect} itemStyles={itemStyles} content={children}>
            <DropdownHead onClick={toggle}>
                <div className="dropdown-button__head">
                    <p className="dropdown__title">
                        {title}
                    </p>
                    <div className={`dropdown__icon ${active ? 'icon-arrow-up' : 'icon-arrow-down'}`}></div>
                </div>
            </DropdownHead>
        </Dropdown>
    );
}

