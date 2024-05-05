import { Course } from "../../models/course";
import { useHistoryNavigation } from "../../hooks/historyNavigation";
import { Paths } from "../../models/paths";
import CourseItem from "./CourseItem";

interface ICoursesContainerProps {
    courses?: Course[];
}

export function CoursesContainer({ courses }: ICoursesContainerProps) {
    const anyCourses = courses && courses.length > 0;
    const { toNext } = useHistoryNavigation();

    return (
        <section className="control-list">
            {anyCourses && courses.map(course =>
                <CourseItem course={course} key={course.id} onClick={() => {
                    toNext(Paths.courseViewPath + '/' + course.id);
                }} />
            )}
            {!anyCourses && <p>На данный момент курсы отсутствуют...</p>}
        </section>);
}