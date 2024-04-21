interface IErrorTextProps {
    children?: React.ReactNode;
}

export function ErrorText({ children }: IErrorTextProps) {
    return (
        <p className="text-danger">{children}</p>
    );
}