/* eslint-disable react-hooks/exhaustive-deps */
import { useEffect, useState } from "react";
import { useUserCourseStatus } from "../hooks/courseHooks";
import { ErrorModal } from "../components/ErrorModal";
import { Loader } from "../ui/Loader";
import { useAppSelector } from "../hooks/redux";
import { useHistoryNavigation } from "../hooks/historyNavigation";
import EllipsisDropdown from "../components/EllipsisDropdown";
import penCircle from "../assets/img/pen-circle.svg"
import crossCircle from "../assets/img/cross-circle.svg"
import { Modal } from "../components/Modal";
import { useGetModule, useRemoveModule } from "../hooks/moduleHooks";
import ModuleEditModal from "./ModuleEditModal";
import { Paths } from "../models/paths";

interface IModuleViewProps {
    courseId: number;
    moduleId: number;
}

function CourseView({ courseId, moduleId }: IModuleViewProps) {
    const { getModuleQuery, module, loading: moduleLoading, error: moduleError, resetValues: resetModuleValues } = useGetModule();
    const { getStatusesQuery, error: statusError, resetError: resetStatusError, isCreator, isSubscriber } = useUserCourseStatus();
    const { removeModuleQuery, loading: removeLoading, error: removeError, success: removeSuccess, resetValues: resetRemoveValues } = useRemoveModule();
    const { toNext } = useHistoryNavigation();

    const { accessToken } = useAppSelector(state => state.authReducer);
    const user = useAppSelector(state => state.userReducer.user);

    const [localError, setLocalError] = useState('');

    const [localLoading, setLocalLoading] = useState(false);

    const [isEditModalActive, setEditModalActive] = useState(false);
    const [removeModalActive, setRemoveModalActive] = useState(false);
    const [updateRequest, setUpdateRequest] = useState(true);

    useEffect(() => {
        if (courseId === 0) {
            return;
        }

        if (updateRequest) {
            fetchModule();
            setUpdateRequest(false);
        }

        fetchUserStatus();

    }, [courseId, user, accessToken, updateRequest]);


    useEffect(() => {
        if (moduleError) setLocalError(moduleError);
        if (statusError) setLocalError(statusError);
    }, [moduleError, statusError]);

    useEffect(() => {
        setLocalLoading(moduleLoading || removeLoading);
    }, [moduleLoading, removeLoading]);

    useEffect(() => {
        if (removeSuccess) {
            returnToCoursePage();
        }
    }, [removeSuccess]);

    async function fetchUserStatus() {
        if (user && accessToken) await getStatusesQuery(user.id!, courseId, accessToken);
    }

    async function fetchModule() {
        if (courseId !== 0 && accessToken) {
            await getModuleQuery(moduleId, accessToken);
        }
    }

    async function removeModule() {
        if (user && module && accessToken) await (removeModuleQuery(module.id, accessToken));
    }

    function resetError() {
        resetModuleValues();
        resetStatusError();
        setLocalError('');
    }

    function returnToCoursePage() {
        toNext(`${Paths.courseViewPath}/${courseId}`);
    }

    return (
        <>
            {localLoading && <Loader />}

            {!localError && !localLoading && module && (isCreator || isSubscriber) &&
                <section className="course-view">
                    <div className="course-view__header container__header">
                        <h2 className="course-view__title medium-big">{module.title}</h2>
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
                            {module.description}
                        </p>
                    </div>

                    <div className="course-view__footer">
                    </div>
                </section >
            }

            {isCreator && module &&
                <>
                    < ModuleEditModal
                        active={isEditModalActive}
                        onClose={() => setEditModalActive(false)}
                        refreshRequest={() => setUpdateRequest(true)}
                        defaultModule={module}
                    />

                    <Modal title="Удаление модуля" active={removeModalActive} onClose={() => setRemoveModalActive(false)}>
                        <p className="regular-red" style={{
                            marginBottom: "40px",
                        }}>Вы уверены, что хотите удалить модуль?</p>
                        <nav style={{
                            display: "flex",
                            justifyContent: "flex-end"
                        }}>
                            <button
                                style={{ width: "80px", marginRight: "50px" }}
                                className="button-red"
                                onClick={removeModule}>
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
                returnToCoursePage();
            }} error={localError} />}
        </>
    );
}

export default CourseView;
