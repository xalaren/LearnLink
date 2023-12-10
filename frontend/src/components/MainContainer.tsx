interface IMainContainerProps {
    title?: string;
    children?: React.ReactNode;
    styles?: React.CSSProperties;
}

export function MainContainer({ title, children, styles }: IMainContainerProps) {
    return (
        <main className="main container" style={styles}>
            <div className="inner-container">
                <h2 className="main__title">{title}</h2>
                {children}
            </div>
        </main>
    )
}