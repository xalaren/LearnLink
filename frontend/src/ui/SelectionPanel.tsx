interface ISelectionPanelProps {
    selectionItems: { title: string, active: boolean, onClick?: () => void }[];
}

function SelectionPanel({ selectionItems }: ISelectionPanelProps) {

    return (
        <nav className="selection-panel">
            {selectionItems.map((item, index) =>
                <div
                    key={index}
                    className={`selection-item ${item.active ? 'active' : ''}`}
                    onClick={() => item.onClick?.()}>
                    {item.title}
                </div>
            )}
        </nav>
    );
}

export default SelectionPanel;