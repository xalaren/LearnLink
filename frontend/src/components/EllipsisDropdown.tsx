import { IDropdownData } from "../models/interfaces";
import { Dropdown } from "../ui/Dropdown";
import ellipsis from "../assets/img/ellipsis.svg"

interface IEllipsisDropdownProps {
    children: IDropdownData[];
}

function EllipsisDropdown({ children }: IEllipsisDropdownProps) {
    return (
        <div className="ellipsis">
            <Dropdown options={children}>
                <div className="ellipsis__icon">
                    <img src={ellipsis} />
                </div>
            </Dropdown>
        </div>

    );
}

export default EllipsisDropdown;