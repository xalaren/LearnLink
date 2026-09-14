interface IInputProps {
    name: string,
    errorMessage?: string,
    placeholder?: string,
    label?: string,
    value?: string,
    required?: boolean,
    className?: string,
    onChange: (event: React.ChangeEvent) => void
}

export function OutlinedInput({ name, errorMessage, onChange, value, placeholder, label, required = false, className = '' }: IInputProps) {

    return (
        <div className={"form-input"}>
            {label && !errorMessage &&
                <p className={`form-input__label ${required ? 'form-input__label-required' : ''}`}>{label}</p>
            }
            {
                errorMessage &&
                <p className="form-input__error required">{errorMessage}</p>
            }
            <input
                className={`form-input__input ${className} ${errorMessage ? 'outlined-input-danger' : 'outlined-input-line'}`}
                type="text" name={name}
                placeholder={placeholder}
                onChange={onChange}
                value={value}
            />
        </div >
    );
}