import DropdownItem from "../../components/Dropdown/DropdownItem";
import EllipsisDropdown from "../../components/Dropdown/EllipsisDropdown";
import { DropdownState } from "../../contexts/DropdownContext";
import { useHistoryNavigation } from "../../hooks/historyNavigation";
import { Course } from "../../models/course";
import { Objective } from "../../models/objective";
import SectionsViewContainer from "../Sections/SectionsViewContainer";

interface IObjectiveViewProps {
    course: Course;
    objective: Objective;
    deleteModalActive: boolean;
    setDeleteModalActive: (active: boolean) => void;
}

function LessonView({
    course,
    objective,
    deleteModalActive,
    setDeleteModalActive
}: IObjectiveViewProps) {
    const { toNext } = useHistoryNavigation();
    return (
        <>
            <section className="view-page__header">
                <p className="view-page__title big-text">{objective.title}</p>
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

            <div dangerouslySetInnerHTML={{ __html: objective.text }} className="view-page__description ui-text" />
        </>
    );
}

export default LessonView;