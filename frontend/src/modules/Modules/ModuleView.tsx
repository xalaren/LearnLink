import ContentAbout from "../../components/ContentAbout/ContentAbout";
import ContentAboutListItem from "../../components/ContentAbout/ContentAboutListItem";
import DropdownItem from "../../components/Dropdown/DropdownItem";
import EllipsisDropdown from "../../components/Dropdown/EllipsisDropdown";
import ProgressBar from "../../components/ProgressBar";
import { DropdownState } from "../../contexts/DropdownContext";
import { Course } from "../../models/course";
import { Module } from "../../models/module";
import LessonsList from "../Lessons/LessonsList";
import ModuleDeleteModal from "./ModuleDeleteModal";
import ModuleEditModal from "./ModuleEditModal";

interface IModuleViewProps {
    course: Course;
    module: Module;
    updateModalActive: boolean;
    setUpdateModalActive: (active: boolean) => void;
    deleteModalActive: boolean;
    setDeleteModalActive: (active: boolean) => void;

}

function ModuleView({
    course,
    module,
    updateModalActive,
    setUpdateModalActive,
    deleteModalActive,
    setDeleteModalActive
}: IModuleViewProps) {
    return (
        <>
            <section className="view-page__header">
                <p className="view-page__title big-text">{module.title}</p>
                {course.localRole?.manageInternalAccess &&
                    <>
                        <DropdownState>
                            <EllipsisDropdown>
                                <DropdownItem title="Редактировать" className="icon icon-pen-circle" key={1} onClick={() => setUpdateModalActive(true)} />
                                <DropdownItem title="Удалить" className="icon icon-cross-circle" key={2} onClick={() => setDeleteModalActive(true)} />
                            </EllipsisDropdown>
                        </DropdownState>
                    </>
                }
            </section>

            <p className="view-page__description ui-text">
                {module.description}
            </p>


            <section className="view-page__content content-side">
                <LessonsList course={course} module={module} />

                <ContentAbout
                    className="content-side__aside"
                    title="О модуле">


                    {module.completionProgress != undefined &&
                        <>
                            <ContentAboutListItem
                                key={5}>
                                Прогресс выполнения: <span className="text-violet">{module.completionProgress}%</span>
                            </ContentAboutListItem>

                            <ContentAboutListItem
                                key={6}>
                                <ProgressBar progress={module.completionProgress} />
                            </ContentAboutListItem>
                        </>
                    }
                </ContentAbout>
            </section>

            <ModuleEditModal active={updateModalActive} onClose={() => setUpdateModalActive(false)} />
            <ModuleDeleteModal active={deleteModalActive} onClose={() => setDeleteModalActive(false)} courseId={course.id} moduleId={module.id} />
        </>
    );
}

export default ModuleView;