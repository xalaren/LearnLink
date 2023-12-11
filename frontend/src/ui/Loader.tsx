interface ILoaderProps {
    className?: string;
    styles?: React.CSSProperties;
}

export function Loader({ className, styles }: ILoaderProps) {
    const resultClass = "loader " + className;
    return (
        <div className={resultClass} style={styles}>
        </div>
    );
}