import DropdownItem from "../../components/Dropdown/DropdownItem";
import EllipsisDropdown from "../../components/Dropdown/EllipsisDropdown";
import { DropdownState } from "../../contexts/DropdownContext";
import { Course } from "../../models/course";
import { Module } from "../../models/module";

interface IModuleViewProps {
    course: Course;
    module: Module;

}

function ModuleView({ course, module }: IModuleViewProps) {
    return (
        <>
            <section className="view-page__header">
                <p className="view-page__title big-text">{course.title}</p>
                {course.localRole?.viewAccess &&
                    <DropdownState>
                        <EllipsisDropdown>
                            {course.localRole.editAccess &&
                                <DropdownItem title="Редактировать" className="icon icon-pen-circle" key={1} onClick={() => { }} />
                            }

                            {course.localRole.removeAccess &&
                                <DropdownItem title="Удалить" className="icon icon-cross-circle" key={2} onClick={() => { }} />
                            }
                        </EllipsisDropdown>
                    </DropdownState>
                }

            </section>
        </>
    );
}

export default ModuleView;