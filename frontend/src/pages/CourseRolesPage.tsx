import { MainContainer } from "../components/MainContainer.tsx";
import ControlNav from "../components/ControlNav.tsx";
import { useParams } from "react-router-dom";
import { useAppSelector } from "../hooks/redux.ts";
import { useListAllLocalRolesAtCourse } from "../hooks/courseLocalRoleHooks.ts";
import { useEffect, useState } from "react";
import { LocalRole } from "../models/localRole.ts";
import RolesList from "../components/RolesList.tsx";
import { RoleItemModel } from "../models/roleItemModel.ts";
import { Loader } from "../components/Loader/Loader.tsx";
import { ErrorModal } from "../components/Modal/ErrorModal.tsx";
import CreateCourseRoleModal from "../modules/CourseRoles/CreateCourseRoleModal.tsx";
import UpdateLocalRoleModal from "../modules/CourseRoles/UpdateCourseRoleModal.tsx";
import DeleteCourseLocalRoleModal from "../modules/CourseRoles/DeleteCourseRoleModal.tsx";

function CourseRolesPage() {
    const param = useParams<'courseId'>();

    const [roles, setRoles] = useState<LocalRole[]>();
    const [createModalActive, setCreateModalActive] = useState(false);
    const [updateModalActive, setUpdateModalActive] = useState(false);
    const [deleteModalActive, setDeleteModalActive] = useState(false);
    const [selectedLocalRole, setSelectedLocalRole] = useState<LocalRole | null>();

    const { user } = useAppSelector(state => state.userReducer);
    const { accessToken } = useAppSelector(state => state.authReducer);
    const { listLocalRolesQuery, loading, error, resetValues } = useListAllLocalRolesAtCourse();

    useEffect(() => {
        if (createModalActive || selectedLocalRole) return;
        fetchRoles();
    }, [createModalActive, selectedLocalRole]);

    async function fetchRoles() {
        const result = await listLocalRolesQuery(Number(param.courseId), accessToken);

        if (result) {
            setRoles(result);
        }
    }

    return (
        <MainContainer title='Управление ролями курса'>
            {!error && !loading &&
                <>
                    <div className="line-end-container">
                        <ControlNav>
                            <button className="control-nav__add-button button-gray icon-plus"
                                onClick={() => setCreateModalActive(true)}></button>
                        </ControlNav>
                    </div>

                    {roles &&
                        <RolesList>
                            {roles.map(item => new RoleItemModel(item,
                                () => {
                                    setSelectedLocalRole(item)
                                    setUpdateModalActive(true);
                                },
                                () => {
                                    setSelectedLocalRole(item)
                                    setDeleteModalActive(true);
                                }))}
                        </RolesList>
                    }

                    {user &&
                        <CreateCourseRoleModal
                            active={createModalActive}
                            courseId={Number(param.courseId)}
                            userId={user.id}
                            accessToken={accessToken}
                            onClose={() => setCreateModalActive(false)} />
                    }

                    {user && selectedLocalRole &&
                        <>
                            <UpdateLocalRoleModal
                                active={updateModalActive}
                                courseId={Number(param.courseId)}
                                localRole={selectedLocalRole}
                                accessToken={accessToken}
                                userId={user.id}
                                onClose={() => {
                                    setSelectedLocalRole(null);
                                    setUpdateModalActive(false);
                                }} />

                            <DeleteCourseLocalRoleModal
                                active={deleteModalActive}
                                courseId={Number(param.courseId)}
                                localRole={selectedLocalRole}
                                accessToken={accessToken}
                                userId={user.id}
                                onClose={() => {
                                    setSelectedLocalRole(null);
                                    setDeleteModalActive(false);
                                }}
                            />
                        </>
                    }
                </>
            }

            {loading &&
                <Loader />
            }

            {error &&
                <ErrorModal active={Boolean(error)} onClose={resetValues} error={error} />
            }
        </MainContainer>
    )
}

export default CourseRolesPage;