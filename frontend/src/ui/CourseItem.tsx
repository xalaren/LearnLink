import { Course } from "../models/course";

interface ICourseItemProps {
    course: Course;
    onClick: () => void;
}

function CourseItem({ course, onClick }: ICourseItemProps) {
    return (
        <div className="course-item" onClick={onClick}>
            <div className="course-item__main">
                <h3 className="course-item__title">{course.title}</h3>
                {!course.isPublic && <div className="icon-user-lock medium-3"></div>}
            </div>
            <div className="course-item__info">
                <p>Подписчиков: <span className="medium-little-violet">{course.subscribersCount}</span></p>
            </div>
        </div>
    );
}

export default CourseItem;