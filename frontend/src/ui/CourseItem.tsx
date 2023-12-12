import { Course } from "../models/course";
import Locked from "./Locked";

interface ICourseItemProps {
    course: Course;
    onClick: () => void;
}

function CourseItem({ course, onClick }: ICourseItemProps) {
    return (
        <div className="course-item" onClick={onClick}>
            <h3 className="course-item__title">{course.title}</h3>
            {!course.isPublic && <Locked />}
        </div>
    );
}

export default CourseItem;