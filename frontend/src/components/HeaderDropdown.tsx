import { CSSProperties, useContext } from "react";
import { Dropdown } from "./Dropdown";
import DropdownHead from "./DropdownHead";
import { DropdownContext } from "../contexts/DropdownContext";
import profile from '../assets/img/profile_placeholder.svg';

interface IHeaderDropdownProps {
    title: string,
    avatarUrl?: string,
    children: React.ReactNode,
    itemStyles?: CSSProperties;
}

export function HeaderDropdown({ title, avatarUrl, children, itemStyles }: IHeaderDropdownProps) {
    const { active, toggle, deselect } = useContext(DropdownContext);

    let imageUrl = avatarUrl || profile;

    return (
        <Dropdown active={active} onDeselect={deselect} itemStyles={itemStyles} content={children} className="header__dropdown">
            <DropdownHead onClick={toggle}>
                <img className="dropdown__profile-image" src={imageUrl} alt="Профиль" />
                <p className="dropdown__profile-name">{title}</p>
                <p className={`icon ${active ? 'icon-arrow-up' : 'icon-arrow-down'}`}></p>
            </DropdownHead>
        </Dropdown>
    );
}

