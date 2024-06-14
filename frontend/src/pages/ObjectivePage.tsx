import { useContext, useState } from "react";
import { MainContainer } from "../components/MainContainer";
import { CourseContext } from "../contexts/CourseContext";
import { ModuleContext } from "../contexts/ModuleContext";
import BreadcrumbItem from "../components/Breadcrumb/BreadcrumbItem";
import { paths } from "../models/paths";
import { ViewTypes } from "../models/enums";
import Breadcrumb from "../components/Breadcrumb/Breadcrumb";
import { Course } from "../models/course";
import { LessonContext } from "../contexts/LessonContext";
import { ObjectiveContext } from "../contexts/ObjectiveContext";
import { Objective } from "../models/objective";
import ObjectiveView from "../modules/Objectives/ObjectiveView";

function ObjectivePage() {
    const { course } = useContext(CourseContext);
    const { module } = useContext(ModuleContext);
    const { lesson } = useContext(LessonContext);
    const { objective } = useContext(ObjectiveContext);

    const [deleteModalActive, setDeleteModalActive] = useState(false);
    const [editModalActive, setEditModalActive] = useState(false);


    return (
        <MainContainer className="view-page">
            <BuildedPage course={course} objective={objective}>
                {course && module && lesson && objective &&
                    <>
                        <Breadcrumb>
                            <BreadcrumbItem text="В начало" path={paths.public()} />
                            {!course!.isPublic &&
                                <BreadcrumbItem text="Мои курсы" path={paths.profile.courses(ViewTypes.created)} />
                            }
                            <BreadcrumbItem text={course!.title} path={paths.course.view.full(course!.id)} />
                            <BreadcrumbItem text={module!.title} path={paths.module.view.full(course!.id, module!.id)} />
                            <BreadcrumbItem text={lesson!.title} path={paths.lesson.view.full(course!.id, module!.id, lesson!.id)} />
                            <BreadcrumbItem text={objective!.title} path={paths.objective.view.full(course!.id, module!.id, lesson!.id, objective!.id)} />
                        </Breadcrumb>

                        <ObjectiveView
                            course={course}
                            lessonId={lesson.id}
                            moduleId={module.id}
                            objective={objective}
                            deleteModalActive={deleteModalActive}
                            setDeleteModalActive={setDeleteModalActive}
                            editModalActive={editModalActive}
                            setEditModalActive={setEditModalActive}
                        />
                    </>
                }
            </BuildedPage>


        </MainContainer >
    );
}

function BuildedPage(props: { course: Course | null, objective: Objective | null, children: React.ReactNode }) {
    if (!props.course) return <p>Не удалось получить доступ к курсу</p>
    if (!props.objective) return <p>Не удалось получить доступ к уроку</p>

    return (
        <>
            {props.children}
        </>
    );
}

export default ObjectivePage;