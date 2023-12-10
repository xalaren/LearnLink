interface IErrorTextProps {
    children?: React.ReactNode;
}

export function ErrorText({ children }: IErrorTextProps) {
    return (
        <p className="regular-red">{children}</p>
    );
}