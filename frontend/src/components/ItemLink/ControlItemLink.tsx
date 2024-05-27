import LoadedCheckbox from "../LoadedCheckbox";

interface IControlItemLinkProps {
    title: string;
    checked: boolean;
    loading: boolean;
    onCheck: () => void;
    onUncheck: () => void;
    onClick: () => void;
    iconClassName?: string;
    className?: string;
}

function ControlItemLink({ title, checked, loading, onCheck, onUncheck, iconClassName = "", className = "" }: IControlItemLinkProps) {

    return (
        <div className={`control-item-link ${className}`}>
            <div className="item-link link-light-violet">
                <div className="item-link__info">
                    <span className={"icon-medium-size " + iconClassName}></span>
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