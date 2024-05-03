interface IDropdownHeadProps {
    toggle: () => void;
    children: React.ReactNode;
}

function DropdownHead({ toggle, children }: IDropdownHeadProps) {
    return (
        <div className="dropdown__head" onClick={toggle}>
            {children}
        </div >);
}

export default DropdownHead;