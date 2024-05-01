interface ISelectionItemProps {
    key: React.Key,
    title: string,
    active: boolean,
    onClick: () => void,
    className?: string,
}

function SelectionItem({ key, title, active, onClick, className = '' }: ISelectionItemProps) {
    const activeClassName = active && 'selection-item-selected';
    return (
        <div className={`selection-item ${activeClassName} ${className}`} key={key} onClick={onClick}>
            {title}
        </div>
    );
}

export default SelectionItem;