import MiniLoader from "./Loader/MiniLoader";

interface ICheckboxProps {
    isChecked: boolean;
    isLoading: boolean;
    className?: string;
    onCheck: () => void;
    onUncheck: () => void;
}

function RoundedCheckbox({ isChecked, isLoading, onCheck, onUncheck, className = '' }: ICheckboxProps) {
    return (
        <div className="checkbox-container" onClick={() => {
            if (isChecked) onUncheck();
            else onCheck();
        }}>
            {isLoading ?
                <MiniLoader /> :
                <div className={`checkbox-rounded ${isChecked && 'checkbox-checked'} ${className}`}>
                    <div className="checkbox__check icon-check icon-normal-size">
                    </div>
                </div>
            }
        </div>
    );
}

export default RoundedCheckbox;