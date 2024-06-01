interface ISectionViewProps {
    sectionTitle?: string;
    children: React.ReactNode;
}

function SectionView({ children, sectionTitle = "" }: ISectionViewProps) {
    return (
        <section className="lesson-section">
            <h3 className="lesson-section__title">{sectionTitle}</h3>
            <p className="lesson-section__text">
                {children}
            </p>
        </section>
    );
}

export default SectionView;