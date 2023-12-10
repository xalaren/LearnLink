import { InputType } from "../helpers/enums";

interface IInputProps {
    type: InputType,
    name: string,
    width?: number,
    errorMessage?: string,
    children?: string,
    onChange: (event: React.ChangeEvent) => void
}

export function Input({ type, name, width = 300, errorMessage, children, onChange }: IInputProps) {
    return (
        <li className="login-input">
            {errorMessage &&
                <div className="regular-red required">{errorMessage}</div>
            }
            <input
                className={errorMessage ? 'red-input' : 'violet-input'}
                type={type} name={name}
                placeholder={children}
                style={{ width: width + 'px' }}
                onChange={onChange} />
        </li>
    );
}