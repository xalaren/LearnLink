import { ILinkData } from "../helpers/interfaces";
import { Arrow } from "./Arrow";
import { useEffect, useState } from "react";

interface IDropdownButtonProps {
    title: string,
    children?: ILinkData[],
}

export function DropdownButton({ title, children }: IDropdownButtonProps) {
    const [active, setActive] = useState(false);

    useEffect(() => {
        const closeDropdown = (event: MouseEvent) => {
            // Проверяем, был ли клик выполнен вне компонента DropdownButton
            const target = event.target as Node;
            const dropdownNode = document.querySelector('.dropdown');

            if (dropdownNode && !dropdownNode.contains(target)) {
                setActive(false);
            }
        };

        document.addEventListener('click', closeDropdown);
        return () => {
            document.removeEventListener('click', closeDropdown);
        };
    }, []);


    return (
        <div className="dropdown">
            <div className="dropdown__head" onClick={() => setActive(prev => !prev)}>
                <p className="dropdown__title">{title}</p>
                <Arrow className="dropdown__arrow" />
            </div>

            {active &&
                <ul className="dropdown__items">
                    {children && children.map(child =>
                        <li className="dropdown__item" onClick={() => {
                            if (child.onClick) child.onClick();
                            setActive(false);
                        }}>
                            {child.title}
                        </li>)
                    }
                </ul>
            }
        </div >
    )
}

