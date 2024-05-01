interface ISelectionPanelProps {
    children: React.ReactNode;
    className?: string;
}

function SelectionPanel({ children, className = '' }: ISelectionPanelProps) {

    return (
        <nav className={`selection-panel ${className}`}>
            {children}
        </nav>
    );
}

export default SelectionPanel;