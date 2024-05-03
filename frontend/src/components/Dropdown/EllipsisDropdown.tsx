import { Dropdown } from "./Dropdown";
import { DropdownContext } from "../../contexts/DropdownContext";
import { useContext } from "react";
import DropdownHead from "./DropdownHead";

interface IEllipsisDropdownProps {
    children: React.ReactNode;
}

function EllipsisDropdown({ children }: IEllipsisDropdownProps) {
    const { active, toggle, deselect } = useContext(DropdownContext);
    return (
        <Dropdown active={active} onDeselect={deselect} content={children} className="ellipsis__dropdown">
            <DropdownHead toggle={toggle}>
                <button className="ellipsis-button"><span className="icon-ellipsis"></span></button>
            </DropdownHead>
        </Dropdown>

    );
}

export default EllipsisDropdown;