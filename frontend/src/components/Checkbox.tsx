interface ICheckboxProps {
    isChecked: boolean;
    label?: string;
    className?: string;
    labelClassName?: string;
    checkedChanger: () => void;
    children?: React.ReactNode;
}

function Checkbox({ isChecked, checkedChanger, label, children, className = '', labelClassName = '' }: ICheckboxProps) {
    return (
        <div className="checkbox-container" >
            <div className={`checkbox ${isChecked && 'checkbox-checked'} ${className}`} onClick={checkedChanger}>
                <div className="checkbox__check icon-check icon-normal-size">
                </div>
            </div>
            <p className={`checkbox__label ${labelClassName}`} onClick={checkedChanger}>{label}</p>
            {children}
        </div>
    );
}

export default Checkbox;