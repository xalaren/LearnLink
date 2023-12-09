import {Course} from "../models/Course.ts";
import {CourseItem} from "./CourseItem.tsx";

interface ICoursesContainerProps {
    courses: Course[];
}

export function CoursesContainer({courses}: ICoursesContainerProps) {
    if (courses.length == 0) return <p>Нет доступных курсов</p>;

    return(
        <section className="course-container">
            {courses.map(course => <CourseItem course={course} key={course.id} />)}
        </section>);
}