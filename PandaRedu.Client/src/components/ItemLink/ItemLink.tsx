interface IItemLinkProps {
    title: string;
    checked: boolean;
    onClick: () => void;
    iconClassName?: string;
    className?: string;
}

function ItemLink({ title, checked, onClick, iconClassName = '', className = '' }: IItemLinkProps) {
    return (
        <div className={`item-link link-light-violet ${className}`} onClick={onClick}>
            <div className="item-link__info">
                <span className={iconClassName}></span>
                {title}
            </div>
            <div className="item-link__properties">
                {checked &&
                    <span className="icon-check icon-medium-size"></span>
                }
            </div>
        </div>
    );
}

export default ItemLink;