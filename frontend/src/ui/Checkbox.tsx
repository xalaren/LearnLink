interface ICheckboxProps {
    isChecked: boolean;
    label?: string;
    className?: string;
    labelClassName?: string;
    checkedChanger: () => void;
}

function Checkbox({ isChecked, checkedChanger, label, className = '', labelClassName = '' }: ICheckboxProps) {
    return (
        <div className="checkbox-container">
            <div className={`checkbox ${isChecked && 'checkbox-checked'} ${className}`}
                onClick={checkedChanger}>
                <div className="checkbox__check icon-check icon-normal-size">
                </div>
            </div>
            <p className={labelClassName}>{label}</p>
        </div>
        // <div className="checkbox-container" onClick={checkedChanger}>
        //     <div className={`checkbox ${isChecked ? 'checked' : ''}`}>
        //         <div className="checkbox__icon icon-check">
        //         </div>
        //     </div>
        //     <p className="medium-little">{label}</p>
        // </div >
    );
}

export default Checkbox;