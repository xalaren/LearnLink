import FileContent from "../../components/Content/FileContent";
import DropdownItem from "../../components/Dropdown/DropdownItem";
import EllipsisDropdown from "../../components/Dropdown/EllipsisDropdown";
import { DropdownState } from "../../contexts/DropdownContext";
import { useHistoryNavigation } from "../../hooks/historyNavigation";
import { Course } from "../../models/course";
import { FileInfo } from "../../models/fileInfo";
import { Objective } from "../../models/objective";
import AnswersList from "../Answers/AnswersList";
import ObjectiveDeleteModal from "./ObjectiveDeleteModal";
import ObjectiveEditModal from "./ObjectiveEditModal";

interface IObjectiveViewProps {
    course: Course;
    moduleId: number;
    lessonId: number;
    objective: Objective;
    editModalActive: boolean;
    setEditModalActive: (active: boolean) => void;
    deleteModalActive: boolean;
    setDeleteModalActive: (active: boolean) => void;
}

function ObjectiveView({
    course,
    moduleId,
    lessonId,
    objective,
    editModalActive,
    setEditModalActive,
    deleteModalActive,
    setDeleteModalActive
}: IObjectiveViewProps) {

    return (
        <>
            <section className="view-page__header">
                <p className="view-page__title big-text">{objective.title}</p>
                {course.localRole?.manageInternalAccess &&
                    <>
                        <DropdownState>
                            <EllipsisDropdown>
                                <DropdownItem
                                    title="Редактировать"
                                    className="icon icon-pen-circle"
                                    key={1}
                                    onClick={() => { setEditModalActive(true) }}
                                />
                                <DropdownItem
                                    title="Удалить"
                                    className="icon icon-cross-circle"
                                    key={2}
                                    onClick={() => setDeleteModalActive(true)}
                                />
                            </EllipsisDropdown>
                        </DropdownState>
                    </>
                }
            </section>

            <div dangerouslySetInnerHTML={{ __html: objective.text }} className="view-page__description ui-text" />

            {objective.fileName && objective.fileExtension && objective.fileUrl &&
                <FileContent key={objective.fileContentId}>
                    {[
                        new FileInfo(
                            objective.fileName,
                            objective.fileExtension,
                            objective.fileUrl
                        )
                    ]}
                </FileContent>
            }

            <AnswersList
                course={course}
                lessonId={lessonId}
                moduleId={moduleId}
                objectiveId={objective.id}
            />

            <ObjectiveDeleteModal
                active={deleteModalActive}
                courseId={course.id}
                lessonId={lessonId}
                moduleId={moduleId}
                objectiveId={objective.id}
                onClose={() => setDeleteModalActive(false)}
            />

            <ObjectiveEditModal
                active={editModalActive}
                onClose={() => setEditModalActive(false)}
            />
        </>
    );
}

export default ObjectiveView;