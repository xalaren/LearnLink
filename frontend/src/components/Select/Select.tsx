import { useEffect } from "react";
import { validate } from "../../helpers/validation";

interface ISelectProps {
    children: React.ReactNode;
    active: boolean;
    defaultTitle: string;
    selectedTitle?: string;
    toggle: () => void;
    onDeselect: () => void;
}

function Select({ defaultTitle, selectedTitle, children, active, toggle, onDeselect }: ISelectProps) {
    let title = validate(selectedTitle || '') ? selectedTitle : defaultTitle;
    const selectedClassName = active ? 'select-selected' : '';

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
        <div className={`select  ${selectedClassName}`}>
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

function validata(selectedTitle: string | undefined) {
    throw new Error("Function not implemented.");
}
