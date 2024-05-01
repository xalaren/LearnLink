interface ICheckboxProps {
    isChecked: boolean;
    label?: string;
    className?: string;
    labelClassName?: string;
    checkedChanger: () => void;
}

function Checkbox({ isChecked, checkedChanger, label, className = '', labelClassName = '' }: ICheckboxProps) {
    return (
        <div className="checkbox-container" onClick={checkedChanger}>
            <div className={`checkbox ${isChecked && 'checkbox-checked'} ${className}`}>
                <div className="checkbox__check icon-check icon-normal-size">
                </div>
            </div>
            <p className={`checkbox__label ${labelClassName}`}>{label}</p>
        </div>
    );
}

export default Checkbox;