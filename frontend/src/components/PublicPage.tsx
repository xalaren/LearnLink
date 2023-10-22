import {CourseItem} from "./CourseItem.tsx";
import {usePublicCourses} from "../hooks/PublicCoursesHook.ts";
import {Loader} from "./Loader.tsx";

export function PublicPage() {
    const {loading, error, courses} = usePublicCourses();

    return (
        <main className="main container">
            <div className="inner-container">
                <h2 className="main__title">Общедоступные курсы: </h2>
                <section className="course-container">
                    {loading && <Loader/>}

                    {!loading && courses.map(course => <CourseItem course={course} key={course.id}/>) }

                    {courses.length == 0 && !loading &&
                        <p>Нет доступных курсов</p>
                    }

                    {/*For error modal view*/}
                </section>

            </div>

        </main>
    )
}