import { Course } from "../models/course";

interface ICourseItemProps {
    course: Course;
    onClick: () => void;
    className?: string;
}

function CourseItem({ course, onClick, className = "" }: ICourseItemProps) {
    return (
        <div className={`${className} course-item`} onClick={onClick}>
            <div className="course-item__info">
                <div className="course-item__head">
                    <p className="course-item__title">{course.title}</p>
                    <div className="course-item__categories">
                        <p>Дата создания: </p>
                        <p className="linked-text">{course.creationDate.toString()} </p>
                    </div>
                </div>
                <div className="course-item__additional">
                    <span className="course-item__icon icon-user"></span>
                    {course.subscribersCount}
                </div>
            </div>
            <div className="course-item__properties">
                {!course.isPublic && <span className="course-item__icon icon-invisible"></span>}
                {course.isUnavailable && <span className="course-item__icon icon-user-lock"></span>}
            </div>
        </div>
    );
}

export default CourseItem;