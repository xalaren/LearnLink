function ContentAboutListItem(props: { title?: string, children: React.ReactNode }) {
    return (
        <div className="content-about__list-item">
            {props.title}
            {props.children}
        </div>
    );
}

export default ContentAboutListItem;