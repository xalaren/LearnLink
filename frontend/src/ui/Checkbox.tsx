interface ICheckboxProps {
    label: string;
    isChecked: boolean;
    checkedChanger: () => void;
}

function Checkbox({ label, isChecked, checkedChanger }: ICheckboxProps) {
    return (
        <div className="checkbox-container" onClick={checkedChanger}>
            <div className={`checkbox ${isChecked ? 'checked' : ''}`}>
                <div className="checkbox__icon icon-check">
                </div>
            </div>
            <p className="medium-little">{label}</p>
        </div >
    );
}

export default Checkbox;