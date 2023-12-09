interface IMainContainerProps {
    title?: string;
    children?: React.ReactNode;
}

export function MainContainer({ title, children }: IMainContainerProps) {
    return (
        <main className="main container">
            <div className="inner-container">
                <h2 className="main__title">{title}</h2>
                {children}
            </div>
        </main>
    )
}