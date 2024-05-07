import { useEffect } from "react";

interface ISelectProps {
    children: React.ReactNode;
    active: boolean;
    defaultTitle: string;
    selectedTitle?: string;
    toggle: () => void;
    onDeselect: () => void;
}

function Select({ defaultTitle, selectedTitle, children, active, toggle, onDeselect }: ISelectProps) {
    let title = selectedTitle || defaultTitle;
    useEffect(() => {
        const closeSelect = (event: MouseEvent) => {
            const target = event.target as HTMLElement;

            if (!target.closest('.select')) {
                onDeselect();
            }

        };

        document.addEventListener('click', closeSelect);
        return () => {
            document.removeEventListener('click', closeSelect);
        };
    }, [onDeselect]);

    return (
        <div className={`select  ${active && 'select-selected'}`}>
            <div className="select__head" onClick={toggle}>
                {title}
                <span className={`icon icon-medium-size ${active ? 'icon-arrow-up' : 'icon-arrow-down'}`}></span>
            </div>
            <div className="select__items">
                {children}
            </div>
        </div>
    );
}

export default Select;