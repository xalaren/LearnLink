import { CSSProperties, useContext } from "react";
import { Dropdown } from "../ui/Dropdown";
import DropdownHead from "../ui/DropdownHead";
import { DropdownContext } from "../contexts/DropdownContext";

interface IHeaderDropdownProps {
    title: string,
    children: React.ReactNode,
    itemStyles?: CSSProperties;
}

export function HeaderDropdown({ title, children, itemStyles }: IHeaderDropdownProps) {
    const { active, toggle, deselect } = useContext(DropdownContext);
    return (
        <Dropdown active={active} onDeselect={deselect} itemStyles={itemStyles} content={children} className="header__dropdown">
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

