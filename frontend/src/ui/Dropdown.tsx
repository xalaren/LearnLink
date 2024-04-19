import { CSSProperties, useEffect } from "react";

interface IDropdownProps {
    onDeselect: () => void;
    active: boolean;
    children: React.ReactNode;
    content: React.ReactNode;
    itemStyles?: CSSProperties;
    className?: string;
}

export function Dropdown({ onDeselect, active, children, content, itemStyles, className }: IDropdownProps) {
    useEffect(() => {
        const closeDropdown = (event: MouseEvent) => {
            const target = event.target as HTMLElement;

            if (!target.closest('.dropdown')) {
                onDeselect();
            }

        };

        document.addEventListener('click', closeDropdown);
        return () => {
            document.removeEventListener('click', closeDropdown);
        };
    }, [onDeselect]);


    return (
        <div className={"dropdown" + " " + className}>
            {children}
            {active &&
                <div className="dropdown__items" style={itemStyles}>
                    {content}
                </div>
            }
        </div >
    )
}