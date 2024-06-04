function ContentAboutListItem(props: { title?: string, children: React.ReactNode }) {
    return (
        <p className="content-about__list-item">
            {props.title}
            {props.children}
        </p>
    );
}

export default ContentAboutListItem;