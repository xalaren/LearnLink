interface ICourseSidebarProps {
    title?: string;
    children: React.ReactNode;
}

function CourseSidebar({ title, children }: ICourseSidebarProps) {
    return (
        <aside className="course-view__sidebar">
            {title && <h3 className="course-sidebar__title">{title}</h3>}
            {children}
        </aside>
    );
}

export default CourseSidebar;