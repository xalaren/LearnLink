import { useParams } from "react-router-dom";
import { useContext, useEffect, useState } from "react";
import { MainContainer } from "../components/MainContainer";
import { CourseContext } from "../contexts/CourseContext";
import { ModuleContext } from "../contexts/ModuleContext";
import BreadcrumbItem from "../components/Breadcrumb/BreadcrumbItem";
import { paths } from "../models/paths";
import { ViewTypes } from "../models/enums";
import Breadcrumb from "../components/Breadcrumb/Breadcrumb";
import { Course } from "../models/course";
import { Module } from "../models/module";

function ModulePage() {
    const { course, fetchCourse } = useContext(CourseContext);
    const { module, fetchModule } = useContext(ModuleContext);

    return (
        <MainContainer className="view-page">
            {course ?
                <>
                    <Breadcrumb>
                        <BreadcrumbItem text="В начало" path={paths.public()} />
                        {!course.isPublic &&
                            <BreadcrumbItem text="Мои курсы" path={paths.profile.courses(ViewTypes.created)} />
                        }
                        <BreadcrumbItem text={course.title} path={paths.course.view.full(course.id)} />
                        <BreadcrumbItem text={module.title} path={paths.course.view.full(course.id)} />
                    </Breadcrumb>
                </> :
                <p>Не удалось получить доступ к курсу...</p>
            }
        </MainContainer >
    );
}

function BuildedPage(props: { course?: Course, module?: Module, children: React.ReactNode }) {
    if (!props.course) return <p>Не удалось получить доступ к курсу</p>
    if (!props.module) return <p>Не удалось получить доступ к модулю</p>

    return (<>

    </>);
}

export default ModulePage;