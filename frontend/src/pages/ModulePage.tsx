import { useContext, useState } from "react";
import { MainContainer } from "../components/MainContainer";
import { CourseContext } from "../contexts/CourseContext";
import { ModuleContext } from "../contexts/ModuleContext";
import BreadcrumbItem from "../components/Breadcrumb/BreadcrumbItem";
import { paths } from "../models/paths";
import { ViewTypes } from "../models/enums";
import Breadcrumb from "../components/Breadcrumb/Breadcrumb";
import { Course } from "../models/course";
import { Module } from "../models/module";
import ModuleView from "../modules/Modules/ModuleView";

function ModulePage() {
    const { course, fetchCourse } = useContext(CourseContext);
    const { module, fetchModule } = useContext(ModuleContext);

    const [updateModalActive, setUpdateModalActive] = useState(false);
    const [deleteModalActive, setDeleteModalActive] = useState(false);

    async function update() {
        await fetchModule();
        await fetchCourse();
    }

    return (
        <MainContainer className="view-page">
            <BuildedPage course={course} module={module}>
                {course && module &&
                    <>
                        <Breadcrumb>
                            <BreadcrumbItem text="В начало" path={paths.public()} />
                            {!course!.isPublic &&
                                <BreadcrumbItem text="Мои курсы" path={paths.profile.courses(ViewTypes.created)} />
                            }
                            <BreadcrumbItem text={course!.title} path={paths.course.view.full(course!.id)} />
                            <BreadcrumbItem text={module!.title} path={paths.course.view.full(course!.id)} />
                        </Breadcrumb>

                        <ModuleView
                            course={course}
                            module={module}
                            updateModalActive={updateModalActive}
                            setUpdateModalActive={setUpdateModalActive}
                            deleteModalActive={deleteModalActive}
                            setDeleteModalActive={setDeleteModalActive}
                            updateSignal={update}
                        />
                    </>
                }
            </BuildedPage>


        </MainContainer >
    );
}

function BuildedPage(props: { course: Course | null, module: Module | null, children: React.ReactNode }) {
    if (!props.course) return <p>Не удалось получить доступ к курсу</p>
    if (!props.module) return <p>Не удалось получить доступ к модулю</p>

    return (
        <>
            {props.children}
        </>
    );
}

export default ModulePage;