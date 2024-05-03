interface IContentAboutProps {
    title: string;
    children: React.ReactNode;
    className?: string;
}

function ContentAbout({ title, children, className = '' }: IContentAboutProps) {
    return (
        <aside className={`content-about ${className}`}>
            <h3 className="content-about__title">{title}</h3>
            <ul className="content-about__list">
                {children}
            </ul>
        </aside>
    );
}

export default ContentAbout;