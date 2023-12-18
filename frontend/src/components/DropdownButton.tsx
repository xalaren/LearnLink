import { CSSProperties } from "react";
import { IDropdownData } from "../models/interfaces";
import { ArrowIcon } from "../ui/ArrowIcon";
import { Dropdown } from "../ui/Dropdown";

interface IDropdownButtonProps {
    title: string,
    children: IDropdownData[],
    itemStyles?: CSSProperties;
}

export function DropdownButton({ title, children, itemStyles }: IDropdownButtonProps) {
    return (
        <Dropdown options={children} itemStyles={itemStyles}>
            <div className="dropdown-button__head">
                <p className="dropdown__title">{title}</p>
                <ArrowIcon className="dropdown__arrow" />
            </div>
        </Dropdown>
    );
}

