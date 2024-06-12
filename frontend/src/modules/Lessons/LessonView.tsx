import CodeContent from "../../components/Content/CodeContent";
import FileContent from "../../components/Content/FileContent";
import TextContent from "../../components/Content/TextContent";
import DropdownItem from "../../components/Dropdown/DropdownItem";
import EllipsisDropdown from "../../components/Dropdown/EllipsisDropdown";
import SectionView from "../../components/Sections/SectionView";
import { DropdownState } from "../../contexts/DropdownContext";
import { useHistoryNavigation } from "../../hooks/historyNavigation";
import { Course } from "../../models/course";
import { FileInfo } from "../../models/fileInfo";
import { Lesson } from "../../models/lesson";
import { Module } from "../../models/module";
import { paths } from "../../models/paths";
import ObjectivesList from "../Objectives/ObjectivesList";
import SectionsViewContainer from "../Sections/SectionsViewContainer";
import LessonDeleteModal from "./LessonDeleteModal";

interface ILessonViewProps {
    course: Course;
    module: Module;
    lesson: Lesson;
    deleteModalActive: boolean;
    setDeleteModalActive: (active: boolean) => void;
}

function LessonView({
    course,
    module,
    lesson,
    deleteModalActive,
    setDeleteModalActive
}: ILessonViewProps) {
    const { toNext } = useHistoryNavigation();
    return (
        <>
            <section className="view-page__header">
                <p className="view-page__title big-text">{lesson.title}</p>
                {course.localRole?.manageInternalAccess &&
                    <>
                        <DropdownState>
                            <EllipsisDropdown>
                                <DropdownItem title="Редактировать" className="icon icon-pen-circle" key={1} onClick={() => toNext(paths.lesson.edit.full(course!.id, module!.id, lesson!.id))} />
                                <DropdownItem title="Удалить" className="icon icon-cross-circle" key={2} onClick={() => setDeleteModalActive(true)} />
                            </EllipsisDropdown>
                        </DropdownState>
                    </>
                }
            </section>

            <p className="view-page__description ui-text">
                {lesson.description}
            </p>

            <SectionsViewContainer />

            <ObjectivesList lessonId={lesson.id} localRole={course.localRole} />

            <LessonDeleteModal
                active={deleteModalActive}
                onClose={() => setDeleteModalActive(false)}
                courseId={course.id} moduleId={module.id}
                lessonId={lesson.id} />
        </>
    );
}

export default LessonView;