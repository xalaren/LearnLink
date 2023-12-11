import { InputType } from "../helpers/enums";

interface IInputProps {
    type: InputType,
    name: string,
    width?: number,
    errorMessage?: string,
    placeholder?: string,
    value?: string,
    onChange: (event: React.ChangeEvent) => void
}

export function Input({ type, name, width = 300, errorMessage, onChange, value, placeholder }: IInputProps) {
    return (
        <li className="login-input">
            {errorMessage &&
                <div className="regular-red required">{errorMessage}</div>
            }
            <input
                className={errorMessage ? 'red-input' : 'violet-input'}
                type={type} name={name}
                placeholder={placeholder}
                style={{ width: width + 'px' }}
                onChange={onChange}
                value={value}
            />
        </li>
    );
}