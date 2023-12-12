import { Course } from "../models/course";
import CourseItem from "../ui/CourseItem";
import { Paths } from "../models/enums";
import { useHistoryNavigation } from "../hooks/historyNavigation";

interface ICoursesContainerProps {
    courses?: Course[];
}

export function CoursesContainer({ courses }: ICoursesContainerProps) {
    const anyCourses = courses && courses.length > 0;
    const { toNext } = useHistoryNavigation();

    return (
        <section className="course-container">
            {anyCourses && courses.map(course => <CourseItem course={course} key={course.id} onClick={() => {
                toNext(Paths.courseViewPath + '/' + course.id);
            }} />)}
            {!anyCourses && <p>На данный момент курсы отсутствуют...</p>}
        </section>);
}