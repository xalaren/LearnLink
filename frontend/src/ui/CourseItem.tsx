import { Course } from "../models/course";
import Locked from "./Locked";

interface ICourseItemProps {
    course: Course;
}

function CourseItem({ course }: ICourseItemProps) {
    return (
        <div className="course-item">
            <h3 className="course-item__title">{course.title}</h3>
            {!course.isPublic && <Locked />}
        </div>
    );
}

export default CourseItem;