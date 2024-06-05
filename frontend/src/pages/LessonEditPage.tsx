import { useContext, useEffect, useState } from "react";
import { MainContainer } from "../components/MainContainer";
import { CourseContext } from "../contexts/CourseContext";
import { ModuleContext } from "../contexts/ModuleContext";
import BreadcrumbItem from "../components/Breadcrumb/BreadcrumbItem";
import { paths } from "../models/paths";
import { ViewTypes } from "../models/enums";
import Breadcrumb from "../components/Breadcrumb/Breadcrumb";
import { Course } from "../models/course";
import { LessonContext } from "../contexts/LessonContext";
import { Lesson } from "../models/lesson";
import LessonCreator from "../modules/Lessons/LessonCreator";
import SectionsEditContainer from "../modules/Sections/SectionsEditContainer";

function LessonPage() {
    const { course } = useContext(CourseContext);
    const { module } = useContext(ModuleContext);
    const { lesson } = useContext(LessonContext);

    return (
        <MainContainer className="view-page">
            <BuildedPage course={course} lesson={lesson}>
                {course && module && lesson &&
                    <>
                        <Breadcrumb>
                            <BreadcrumbItem text="В начало" path={paths.public()} />
                            {!course!.isPublic &&
                                <BreadcrumbItem text="Мои курсы" path={paths.profile.courses(ViewTypes.created)} />
                            }
                            <BreadcrumbItem text={course!.title} path={paths.course.view.full(course!.id)} />
                            <BreadcrumbItem text={module!.title} path={paths.module.view.full(course!.id, module!.id)} />
                            <BreadcrumbItem text={lesson!.title} path={paths.lesson.view.full(course!.id, module!.id, lesson!.id)} />
                            <BreadcrumbItem text="Редактирование урока" path={paths.lesson.edit.full(course!.id, module!.id, lesson!.id)} />
                        </Breadcrumb>

                        <LessonCreator />
                    </>
                }
            </BuildedPage>


        </MainContainer >
    );
}

function BuildedPage(props: { course: Course | null, lesson: Lesson | null, children: React.ReactNode }) {
    if (!props.course) return <p>Не удалось получить доступ к курсу</p>
    if (!props.lesson) return <p>Не удалось получить доступ к уроку</p>

    return (
        <>
            {props.children}
        </>
    );
}

export default LessonPage;