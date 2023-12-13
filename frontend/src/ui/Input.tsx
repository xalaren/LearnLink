import { InputType } from "../models/enums";

interface IInputProps {
    type: InputType,
    name: string,
    width?: number,
    errorMessage?: string,
    placeholder?: string,
    value?: string,
    className?: string,
    styles?: React.CSSProperties,
    onChange: (event: React.ChangeEvent) => void
}

export function Input({ type, name, width = 300, errorMessage, onChange, value, placeholder, className, styles }: IInputProps) {
    const style = {
        ...styles,
        width: width + 'px'
    }

    return (
        <li className="form-input">
            {errorMessage &&
                <div className="regular-red required">{errorMessage}</div>
            }
            <input
                className={`${errorMessage ? 'red-input' : 'violet-input'} ${className}`}
                type={type} name={name}
                placeholder={placeholder}
                style={style}
                onChange={onChange}
                value={value}
            />
        </li>
    );
}