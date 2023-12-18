/* eslint-disable react-hooks/exhaustive-deps */
import { useEffect, useState } from "react";
import { useGetCourse, useRemoveCourse, useUserCourseStatus } from "../hooks/courseHooks";
import { ErrorModal } from "../components/ErrorModal";
import { Loader } from "../ui/Loader";
import { useAppSelector } from "../hooks/redux";
import { useHistoryNavigation } from "../hooks/historyNavigation";
import { Paths, ViewTypes } from "../models/enums";
import CourseInfoSidebar from "./CourseInfoSidebar";
import ModulesContainer from "./ModulesContainer";
import EllipsisDropdown from "../components/EllipsisDropdown";
import penCircle from "../assets/img/pen-circle.svg"
import crossCircle from "../assets/img/cross-circle.svg"
import CourseEditModule from "./CourseEditModule";
import { Modal } from "../components/Modal";

interface ICourseViewProps {
    courseId: number;
}

function CourseView({ courseId }: ICourseViewProps) {
    const { getCourseQuery, course, loading: courseLoading, error: courseError, resetValues: resetCourseValues } = useGetCourse();
    const { getStatusesQuery, error: statusError, resetError: resetStatusError, isCreator, isSubscriber } = useUserCourseStatus();
    const { removeCourseQuery, loading: removeLoading, error: removeError, success: removeSuccess, resetValues: resetRemoveValues } = useRemoveCourse();
    const { toNext } = useHistoryNavigation();

    const { accessToken } = useAppSelector(state => state.authReducer);
    const user = useAppSelector(state => state.userReducer.user);

    const [localError, setLocalError] = useState('');
    const [isSubscriptionChanged, setSubscriptionChanged] = useState(false);

    const [localLoading, setLocalLoading] = useState(false);

    const [isEditModalActive, setEditModalActive] = useState(false);
    const [removeModalActive, setRemoveModalActive] = useState(false);
    const [updateRequest, setUpdateRequest] = useState(true);

    useEffect(() => {
        if (courseId === 0) {
            return;
        }

        if (updateRequest) {
            fetchCourse();
            setUpdateRequest(false);
        }

    }, [courseId, user, accessToken, updateRequest]);

    useEffect(() => {
        if (isSubscriptionChanged) {
            fetchCourse();
            setSubscriptionChanged(false);
        }
    }, [isSubscriptionChanged]);


    useEffect(() => {
        if (courseError) setLocalError(courseError);
        if (statusError) setLocalError(statusError);
    }, [courseError, statusError]);

    useEffect(() => {
        setLocalLoading(courseLoading || removeLoading);
    }, [courseLoading, removeLoading]);

    useEffect(() => {
        if (removeSuccess) {
            if (isCreator || isSubscriber) {
                toNext(Paths.userCoursesPath + '/' + ViewTypes.created);
            }
            else {
                toNext(Paths.homePath);
            }
        }
    }, [removeSuccess])

    async function fetchCourse() {
        if (courseId !== 0) {
            await getCourseQuery(courseId, user?.id);
            if (user && accessToken) await getStatusesQuery(user.id!, courseId, accessToken);
        }
    }

    async function removeCourse() {
        if (user && course && accessToken) await (removeCourseQuery(user.id, course.id, accessToken));
    }

    function resetError() {
        resetCourseValues();
        resetStatusError();
        setLocalError('');
    }

    return (
        <>
            {localLoading && <Loader />}

            {!localError && !localLoading && course &&
                <section className="course-view">
                    <div className="course-view__header container__header">
                        <h2 className="course-view__title medium-big">{course.title}</h2>
                        {isCreator && <nav className="container__navigation">
                            <EllipsisDropdown>
                                {[
                                    {
                                        title: "Редактировать",
                                        onClick: () => { setEditModalActive(true) },
                                        iconPath: penCircle
                                    },
                                    {
                                        title: "Удалить",
                                        onClick: () => { setRemoveModalActive(true) },
                                        iconPath: crossCircle,
                                    }
                                ]}
                            </EllipsisDropdown>
                        </nav>}
                    </div>
                    <div className="course-view__description">
                        <p className="description regular">
                            {course.description}
                        </p>
                    </div>

                    <div className="course-view__content">
                        <ModulesContainer allowEdit={isCreator} courseId={course.id} />
                    </div>

                    <CourseInfoSidebar course={course} isSubscriber={isSubscriber} isCreator={isCreator} subscriptionChanged={() => setSubscriptionChanged(true)} />

                    <div className="course-view__footer">
                    </div>
                </section >
            }

            {isCreator && course &&
                <>
                    < CourseEditModule
                        active={isEditModalActive}
                        onClose={() => setEditModalActive(false)}
                        refreshRequest={() => setUpdateRequest(true)}
                        defaultCourse={course}
                    />

                    <Modal title="Удаление курса" active={removeModalActive} onClose={() => setRemoveModalActive(false)}>
                        <p className="regular-red" style={{
                            marginBottom: "40px",
                        }}>Вы уверены, что хотите удалить курс?</p>
                        <nav style={{
                            display: "flex",
                            justifyContent: "flex-end"
                        }}>
                            <button
                                style={{ width: "80px", marginRight: "50px" }}
                                className="button-red"
                                onClick={removeCourse}>
                                Да
                            </button>
                            <button
                                style={{ width: "80px" }}
                                className="button-violet"
                                onClick={() => setRemoveModalActive(false)}>
                                Нет
                            </button>
                        </nav>
                    </Modal>
                </>
            }

            {removeError && <ErrorModal active={Boolean(removeError)} onClose={() => {
                resetRemoveValues();
            }} error={removeError} />}

            {localError && <ErrorModal active={Boolean(localError)} onClose={() => {
                resetError();
                toNext(Paths.homePath);
            }} error={localError} />}
        </>
    );
}

export default CourseView;