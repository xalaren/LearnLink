interface IMainHeaderViaNavProps {
    title: string;
    children?: React.ReactNode;
}

function ContainerHeaderViaNav({ title, children }: IMainHeaderViaNavProps) {
    return (
        <section className="container__header">
            <h2 className="main__title">{title}</h2>
            <nav className="container__navigation">
                {children}
            </nav>
        </section>
    );
}

export default ContainerHeaderViaNav;