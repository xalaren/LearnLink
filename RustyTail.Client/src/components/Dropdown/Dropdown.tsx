import { CSSProperties, useEffect, useRef } from "react";

interface IDropdownProps {
    onDeselect: () => void;
    active: boolean;
    children: React.ReactNode;
    content: React.ReactNode;
    itemStyles?: CSSProperties;
    className?: string;
}

export function Dropdown({ onDeselect, active, children, content, itemStyles, className }: IDropdownProps) {
    const dropdownRef = useRef<HTMLDivElement>(null);
    const itemsRef = useRef<HTMLDivElement>(null);

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

    useEffect(() => {
        if (active && dropdownRef.current && itemsRef.current) {
            const rect = dropdownRef.current.getBoundingClientRect();
            const menuWidth = itemsRef.current.offsetWidth;
            const spaceToRight = window.innerWidth - rect.right;
            const spaceToLeft = rect.left;

            if (spaceToRight >= menuWidth) {
                itemsRef.current.style.left = '0';
                itemsRef.current.style.right = 'auto';
            } else if (spaceToLeft >= menuWidth) {
                itemsRef.current.style.right = '0';
                itemsRef.current.style.left = 'auto';
            } else {
                itemsRef.current.style.left = '0';
                itemsRef.current.style.right = 'auto';
            }
        }
    }, [active]);


    return (
        <div ref={dropdownRef} className={"dropdown" + " " + className}>
            {children}
            {active &&
                <div ref={itemsRef} className="dropdown__items" style={itemStyles}>
                    {content}
                </div>
            }
        </div >
    )
}