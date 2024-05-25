import ContentList from "../../components/ContentList/ContentList";
import { useAppSelector } from "../../hooks/redux";
import { Course } from "../../models/course";
import { Module } from "../../models/module";

interface ILessonsListProps {
    module: Module;
    course: Course;
}

function LessonsList({ module, course }: ILessonsListProps) {
    const { user } = useAppSelector(state => state.userReducer);

    return (
        <ContentList
            className="content-side__main"
            title="Уроки модуля"
            showButton={course.localRole?.manageInternalAccess || false}
            onHeadButtonClick={() => { }}>
            <p></p>
        </ContentList>
    );
}

export default LessonsList;