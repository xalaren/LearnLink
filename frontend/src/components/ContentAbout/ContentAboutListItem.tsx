function ContentAboutListItem(props: { title?: string, children: React.ReactNode }) {
    return (
        <li className="content-about__list-item">
            {props.title}
            {props.children}
        </li>
    );
}

export default ContentAboutListItem;