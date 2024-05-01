import { InputType } from "../models/enums";

interface IInputProps {
    type: InputType,
    name: string,
    width?: number,
    errorMessage?: string,
    placeholder?: string,
    label?: string,
    value?: string,
    className?: string,
    styles?: React.CSSProperties,
    onChange: (event: React.ChangeEvent) => void
}

export function Input({ type, name, errorMessage, onChange, value, placeholder, label, className = '', styles }: IInputProps) {

    return (
        <div className={"form-input " + className}>
            {label && !errorMessage &&
                <p className="form-input__label">{label}</p>
            }
            {errorMessage &&
                <p className="form-input__error required">{errorMessage}</p>
            }
            {type === InputType.rich ?
                <textarea
                    name={name}
                    onChange={onChange}
                    placeholder={placeholder}
                    style={styles}
                    className={`form-input__input textarea ${errorMessage ? 'input-danger' : 'input-gray'}`}
                    value={value}
                /> :
                <input
                    className={`form-input__input ${errorMessage ? 'input-danger' : 'input-gray'}`}
                    type={type} name={name}
                    placeholder={placeholder}
                    style={styles}
                    onChange={onChange}
                    value={value}
                />
            }
        </div>
    );
}