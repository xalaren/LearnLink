interface ICheckboxProps {
    label: string;
    isChecked: boolean;
}

function Checkbox({ label, isChecked }: ICheckboxProps) {
    return (
        <div className="checkbox-container">
            <div className={`checkbox ${isChecked ? 'checked' : ''}`}>
                <div className="checkbox__icon">
                    <svg width="21" height="15" viewBox="0 0 21 15" fill="none" xmlns="http://www.w3.org/2000/svg">
                        <path d="M3 8.55556L8.2 13L16 3" stroke="#8E8E8E" stroke-width="2.2" stroke-linecap="round" stroke-linejoin="round" />
                    </svg>
                </div>
            </div>
            <p className="medium-little">{label}</p>
        </div >
    );
}

export default Checkbox;