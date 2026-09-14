import { useContext, useState } from "react";
import { MainContainer } from "../../components/MainContainer";
import { CourseContext } from "../../contexts/CourseContext";
import { ModuleContext } from "../../contexts/ModuleContext";
import BreadcrumbItem from "../../components/Breadcrumb/BreadcrumbItem";
import { paths } from "../../models/paths";
import { ViewTypes } from "../../models/enums";
import Breadcrumb from "../../components/Breadcrumb/Breadcrumb";
import { Course } from "../../models/course";
import { LessonContext } from "../../contexts/LessonContext";
import { ObjectiveContext } from "../../contexts/ObjectiveContext";
import { AnswerContext } from "../../contexts/AnswerContext";
import { DropdownState } from "../../contexts/DropdownContext";
import EllipsisDropdown from "../../components/Dropdown/EllipsisDropdown";
import DropdownItem from "../../components/Dropdown/DropdownItem";
import ContentAbout from "../../components/ContentAbout/ContentAbout";
import ContentAboutListItem from "../../components/ContentAbout/ContentAboutListItem";
import { Answer } from "../../models/answer";
import { useAppSelector } from "../../hooks/redux";
import { User } from "../../models/user";
import FileContent from "../../components/Content/FileContent";
import { FileInfo } from "../../models/fileInfo";
import AnswerDeleteModal from "../../modules/Answers/AnswerDeleteModal";
import AnswerEditModal from "../../modules/Answers/AnswerEditModal";
import ReviewView from "../../modules/Reviews/ReviewView";
import CreateReviewModal from "../../modules/Reviews/CreateReviewModal";

function AnswerViewPage() {
    const { course } = useContext(CourseContext);
    const { module } = useContext(ModuleContext);
    const { lesson } = useContext(LessonContext);
    const { objective } = useContext(ObjectiveContext);
    const { answer, fetchAnswer } = useContext(AnswerContext);

    const { user } = useAppSelector(state => state.userReducer);

    const [deleteModalActive, setDeleteModalActive] = useState(false);
    const [updateModalActive, setUpdateModalActive] = useState(false);
    const [createReviewModalActive, setCreateReviewModalActive] = useState(false);


    return (
        <MainContainer className="view-page">
            <BuildedPage
                course={course}
                answer={answer}
                user={user || null}
            >
                {course && module && lesson && objective && answer && course.localRole &&
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
                            <BreadcrumbItem text={`Ответ на задание ${objective.id} (${answer.userDetails.nickname})`} path={paths.answer.view.full(course!.id, module!.id, lesson!.id, objective!.id, answer!.id)} />
                        </Breadcrumb>

                        <section className="view-page__header">
                            <p className="view-page__title ui-title">{`Ответ на "${objective.title}"`}</p>
                            {course.localRole.viewAccess &&
                                <>
                                    <DropdownState>
                                        <EllipsisDropdown>
                                            {course.localRole.manageInternalAccess && !answer.grade &&
                                                <DropdownItem
                                                    title="Оценить"
                                                    className="icon icon-star"
                                                    key={1}
                                                    onClick={() => { setCreateReviewModalActive(true) }}
                                                />
                                            }
                                            <DropdownItem
                                                title="Редактировать"
                                                className="icon icon-pen-circle"
                                                key={3}
                                                onClick={() => setUpdateModalActive(true)}
                                            />

                                            <DropdownItem
                                                title="Удалить"
                                                className="icon icon-cross-circle"
                                                key={4}
                                                onClick={() => setDeleteModalActive(true)}
                                            />
                                        </EllipsisDropdown>
                                    </DropdownState>
                                </>
                            }
                        </section>

                        <section className="view-page__content content-side">
                            <div className="content-side__main indented">
                                <p className="medium-text">Текст ответа:</p>
                                {answer.text ?
                                    <div dangerouslySetInnerHTML={{ __html: answer.text }} className="view-page__description ui-text" /> :
                                    <p className="optional-text">Текст не был заполнен...</p>
                                }

                                <p className="medium-text">Загруженные файлы:</p>
                                {answer.fileDetails ?
                                    <FileContent>
                                        {
                                            [
                                                new FileInfo(
                                                    answer.fileDetails.name,
                                                    answer.fileDetails.extension,
                                                    answer.fileDetails.url,
                                                )
                                            ]
                                        }
                                    </FileContent> :
                                    <p className="optional-text">Файл не был загружен...</p>
                                }
                            </div>

                            <ContentAbout
                                className="content-side__aside"
                                title="Информация об ответе">
                                <ContentAboutListItem
                                    key={1}>
                                    Выполнено:&nbsp;
                                    <span className="text-violet medium-text">{answer.userDetails.lastname} {answer.userDetails.name[0]}.
                                    </span>
                                </ContentAboutListItem>

                                <ContentAboutListItem
                                    key={2}>
                                    Дата выполнения:&nbsp;
                                    <span className="text-violet medium-text">
                                        {answer.uploadDate}
                                    </span>
                                </ContentAboutListItem>

                            </ContentAbout>
                        </section>

                        <ReviewView
                            answerId={answer.id}
                            localRole={course.localRole}
                        />

                        <AnswerDeleteModal
                            active={deleteModalActive}
                            answerId={answer.id}
                            courseId={course.id}
                            moduleId={module.id}
                            lessonId={lesson.id}
                            objectiveId={objective.id}
                            onClose={() => setDeleteModalActive(false)}
                        />

                        <AnswerEditModal
                            active={updateModalActive}
                            lessonId={lesson.id}
                            onClose={() => {
                                setUpdateModalActive(false);
                                fetchAnswer();
                            }}
                        />

                        <CreateReviewModal
                            active={createReviewModalActive}
                            answerId={answer.id}
                            onClose={() => {
                                setCreateReviewModalActive(false);
                                fetchAnswer();
                            }}
                        />
                    </>
                }
            </BuildedPage>


        </MainContainer >
    );
}

function BuildedPage(props: {
    course: Course | null,
    answer: Answer | null,
    user: User | null,
    children: React.ReactNode
}) {
    if (!props.course) return <p>Не удалось получить доступ к курсу</p>
    if (!props.answer) return <p>Не удалось получить доступ к ответу</p>
    if (!props.course.localRole) return <p>Не удалось получить роль пользователя</p>
    if (!props.user) return <p>Не удалось определить пользователя</p>
    if (!props.course.localRole.manageInternalAccess && props.user.id !== props.answer.userDetails.id) return <p>Недостаточно прав для просмотра ответа</p>

    return (
        <>
            {props.children}
        </>
    );
}

export default AnswerViewPage;