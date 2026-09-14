interface ISelectionItemProps {
    index: number;
    title: string,
    active: boolean,
    onClick: () => void,
    className?: string,
}

function SelectionItem({ index, title, active, onClick, className = '' }: ISelectionItemProps) {
    const activeClassName = active && 'selection-item-selected';
    return (
        <div className={`selection-item ${activeClassName} ${className}`} key={index} onClick={onClick}>
            {title}
        </div>
    );
}

export default SelectionItem;