import { Dropdown } from "./Dropdown";
import { DropdownContext } from "../contexts/DropdownContext";
import { useContext } from "react";

interface IEllipsisDropdownProps {
    children: React.ReactNode;
}

function EllipsisDropdown({ children }: IEllipsisDropdownProps) {
    const { active, toggle, deselect } = useContext(DropdownContext);
    return (
        <div className="ellipsis">
            <Dropdown active={active} onDeselect={deselect} content={children}>
                <div className="ellipsis__icon icon-ellipsis" onClick={toggle}>
                </div>
            </Dropdown>
        </div>

    );
}

export default EllipsisDropdown;