import { Course } from "../models/course";
import CourseItem from "../ui/CourseItem";

interface ICoursesContainerProps {
    courses?: Course[];
}

export function CoursesContainer({ courses }: ICoursesContainerProps) {
    const anyCourses = courses && courses.length > 0;

    return (
        <section className="course-container">
            {anyCourses && courses.map(course => <CourseItem course={course} key={course.id} />)}
            {!anyCourses && <p>На данный момент курсы отсутствуют...</p>}
        </section>);
}