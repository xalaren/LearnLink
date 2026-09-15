interface IMainContainerProps {
    title?: string;
    children?: React.ReactNode;
    styles?: React.CSSProperties;
    className?: string;
}

export function MainContainer({ title, children, styles, className = "" }: IMainContainerProps) {
    return (
        <main className={"main container common-page-wrapper " + className} style={styles}>
            {title && <h3>{title}</h3>}
            {children}
        </main>
    )
}