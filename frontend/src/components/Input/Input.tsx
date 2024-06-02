import { InputType } from "../../models/enums";

interface IInputProps {
    type: InputType,
    name: string,
    width?: number,
    errorMessage?: string,
    placeholder?: string,
    label?: string,
    value?: string,
    required?: boolean,
    className?: string,
    onChange: (event: React.ChangeEvent) => void
}

export function Input({ type, name, errorMessage, onChange, value, placeholder, label, required = false, className = '' }: IInputProps) {
    const filledClassName = className || 'input-gray';
    return (
        <div className={"form-input"}>
            {label && !errorMessage &&
                <p className={`form-input__label ${required ? 'form-input__label-required' : ''}`}>{label}</p>
            }
            {errorMessage &&
                <p className="form-input__error required">{errorMessage}</p>
            }
            {type === InputType.rich ?
                <textarea
                    name={name}
                    onChange={onChange}
                    placeholder={placeholder}
                    className={`form-input__input textarea ${errorMessage ? 'input-danger' : filledClassName}`}
                    value={value}
                /> :
                <input
                    className={`form-input__input ${errorMessage ? 'input-danger' : filledClassName}`}
                    type={type} name={name}
                    placeholder={placeholder}
                    onChange={onChange}
                    value={value}
                />
            }
        </div>
    );
}