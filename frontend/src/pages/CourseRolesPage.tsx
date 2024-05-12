import {MainContainer} from "../components/MainContainer.tsx";
import ControlNav from "../components/ControlNav.tsx";
import {useParams} from "react-router-dom";
import {useAppSelector} from "../hooks/redux.ts";
import {useListAllLocalRolesAtCourse} from "../hooks/courseLocalRoleHooks.ts";
import {useEffect, useState} from "react";
import {LocalRole} from "../models/localRole.ts";
import RolesList from "../components/RolesList.tsx";
import {RoleItemModel} from "../models/roleItemModel.ts";
import {Loader} from "../components/Loader/Loader.tsx";
import {ErrorModal} from "../components/Modal/ErrorModal.tsx";
import CreateCourseRoleModal from "../modules/CourseRoles/CreateCourseRoleModal.tsx";

function CourseRolesPage() {
    const param = useParams<'courseId'>();

    const [roles, setRoles] = useState<LocalRole[]>();
    const [createModalActive, setCreateModalActive] = useState(false);

    const { user } = useAppSelector(state => state.userReducer);
    const { accessToken } = useAppSelector(state => state.authReducer);
    const { listLocalRolesQuery, loading, error, resetValues } = useListAllLocalRolesAtCourse();

    useEffect(() => {
        fetchRoles();
    }, []);

    async function fetchRoles() {
        const result = await listLocalRolesQuery(Number(param.courseId), accessToken);

        if(result) {
            setRoles(result);
        }
    }

    return(
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
                            {roles.map(item => new RoleItemModel(item, () => {}, () => {}))}
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