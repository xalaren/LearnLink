interface ISwitchProps {
    isChecked: boolean;
    label?: string;
    className?: string;
    labelClassName?: string;
    checkedChanger: () => void;
}

function Switch({ isChecked, checkedChanger, label, className = '', labelClassName = '' }: ISwitchProps) {
    return (
        <div className="switch-container" onClick={checkedChanger}>
            <div className={`switch ${isChecked && 'switch-checked'} ${className}`}>
                <div className="switch__button">
                </div>
            </div>
            <p className={`switch__label ${labelClassName}`}>{label}</p>
        </div>
    );
}

export default Switch;