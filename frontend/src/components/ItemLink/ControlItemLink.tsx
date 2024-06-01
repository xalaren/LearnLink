import LoadedCheckbox from "../LoadedCheckbox";

interface IControlItemLinkProps {
    title: string;
    checked: boolean;
    loading: boolean;
    onCheck: () => void;
    onUncheck: () => void;
    onClick: () => void;
    className?: string;
    children?: React.ReactNode;
}

function ControlItemLink({ title, checked, loading, onCheck, onUncheck, onClick, children, className = "" }: IControlItemLinkProps) {

    return (
        <div className={`control-item-link ${className}`}>
            <div className="item-link link-light-violet" onClick={onClick}>
                <div className="item-link__info">
                    {children}
                    {title}
                </div>
            </div>
            <LoadedCheckbox
                isChecked={checked}
                onCheck={onCheck}
                onUncheck={onUncheck}
                isLoading={loading}
            />
        </div>
    );
}

export default ControlItemLink;