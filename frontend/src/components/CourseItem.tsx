import {Course} from "../models/Course.ts";
interface CourseItemProps {
    course: Course;
}
export function CourseItem({course}: CourseItemProps) {
    return(
        <div className="course-item">
            <h3 className="course-item__title">{course.title}</h3>
        </div>
    );
}