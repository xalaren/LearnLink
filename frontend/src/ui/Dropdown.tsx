import { CSSProperties, useEffect, useState } from "react";
import { IDropdownData } from "../models/interfaces";

interface IDropdownProps {
    children: React.ReactNode;
    options: IDropdownData[];
    itemStyles?: CSSProperties;
}

export function Dropdown({ children, options, itemStyles }: IDropdownProps) {
    const [active, setActive] = useState(false);

    useEffect(() => {
        const closeDropdown = (event: MouseEvent) => {
            const target = event.target as HTMLElement;

            if (!target.closest('.dropdown')) {
                setActive(false);
            }

        };

        document.addEventListener('click', closeDropdown);
        return () => {
            document.removeEventListener('click', closeDropdown);
        };
    }, [active]);


    return (
        <div className="dropdown">
            <div className="dropdown__head" onClick={() => setActive(prev => !prev)}>
                {children}
            </div>

            {active &&
                <ul className="dropdown__items" style={itemStyles}>
                    {options && options.map(option =>
                        <li className="dropdown__item" onClick={() => {
                            if (option.onClick) option.onClick();
                            setActive(false);
                        }} key={options.indexOf(option)}>
                            {option.iconPath &&
                                <img className="dropdown__icon" src={option.iconPath} alt="icon" />}
                            {option.title}
                        </li>)
                    }
                </ul>
            }
        </div >
    )
}