interface IDropdownHeadProps {
    onClick: () => void;
    children: React.ReactNode;
}

function DropdownHead({ onClick, children }: IDropdownHeadProps) {
    return (
        <div className="dropdown__head" onClick={onClick}>
            {children}
        </div>);
}

export default DropdownHead;