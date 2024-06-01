import DropdownItem from "../../components/Dropdown/DropdownItem";
import EllipsisDropdown from "../../components/Dropdown/EllipsisDropdown";
import { DropdownState } from "../../contexts/DropdownContext";
import { Course } from "../../models/course";
import { Lesson } from "../../models/lesson";
import { Module } from "../../models/module";
import LessonDeleteModal from "./LessonDeleteModal";

interface ILessonViewProps {
    course: Course;
    module: Module;
    lesson: Lesson;
    deleteModalActive: boolean;
    setDeleteModalActive: (active: boolean) => void;
}

function ModuleView({
    course,
    module,
    lesson,
    deleteModalActive,
    setDeleteModalActive
}: ILessonViewProps) {
    return (
        <>
            <section className="view-page__header">
                <p className="view-page__title big-text">{lesson.title}</p>
                {course.localRole?.manageInternalAccess &&
                    <>
                        <DropdownState>
                            <EllipsisDropdown>
                                <DropdownItem title="Редактировать" className="icon icon-pen-circle" key={1} onClick={() => { }} />
                                <DropdownItem title="Удалить" className="icon icon-cross-circle" key={2} onClick={() => setDeleteModalActive(true)} />
                            </EllipsisDropdown>
                        </DropdownState>
                    </>
                }
            </section>

            <p className="view-page__description ui-text">
                {lesson.description}
            </p>

            <LessonDeleteModal
                active={deleteModalActive}
                onClose={() => setDeleteModalActive(false)}
                courseId={course.id} moduleId={module.id}
                lessonId={lesson.id} />
        </>
    );
}

export default ModuleView;