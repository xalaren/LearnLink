import { Outlet, useParams } from "react-router-dom";
import { MainContainer } from "../../components/MainContainer";
import SearchForm from "../../components/SearchForm";
import React, { useContext, useEffect, useState } from "react";
import { useHistoryNavigation } from "../../hooks/historyNavigation";
import Paginate from "../../components/Paginate";
import ControlNav from "../../components/ControlNav";
import { LocalRole } from "../../models/localRole";
import { useAppSelector } from "../../hooks/redux";
import { useFindCourseParticipants } from "../../hooks/courseHooks";
import { Participant } from "../../models/participant";
import { Loader } from "../../components/Loader/Loader";
import { ErrorModal } from "../../components/Modal/ErrorModal";
import ParticipantItem from "../../components/UsersList/ParticipantItem";
import { useGetLocalRoleByUserAtCourse } from "../../hooks/userCourseLocalRoleHooks.ts";
import ParticipantsLocalRoleModal from "../../modules/Participants/ParticipantsLocalRoleModal.tsx";
import ParticipantKickModal from "../../modules/Participants/ParticipantKickModal.tsx";
import ParticipantsInviteModal from "../../modules/Participants/ParticipantsInviteModal.tsx";
import Breadcrumb from "../../components/Breadcrumb/Breadcrumb.tsx";
import BreadcrumbItem from "../../components/Breadcrumb/BreadcrumbItem.tsx";
import { paths } from "../../models/paths.ts";
import { ViewTypes } from "../../models/enums.ts";
import { CourseContext } from "../../contexts/CourseContext.tsx";

function CourseParticipantsPage() {
    const courseParam = useParams<'courseId'>();
    const pageParam = useParams<'pageNumber'>();

    const [page, setPage] = useState(Number(pageParam.pageNumber));
    const [pageCount, setPageCount] = useState(1);
    const [searchText, setSearchText] = useState('');

    const { course } = useContext(CourseContext);
    const { user } = useAppSelector(state => state.userReducer);
    const { accessToken } = useAppSelector(state => state.authReducer);

    const [localRole, setLocalRole] = useState<LocalRole>();
    const [participants, setParticipants] = useState<Participant[]>();
    const [kickModalActive, setKickModalActive] = useState(false);
    const [localRoleModalActive, setLocalRoleModalActive] = useState(false);
    const [inviteModalActive, setInviteModalActive] = useState(false);
    const [selectedParticipant, setSelectedParticipant] = useState<Participant | null>(null);

    const findCourseParticipantsHook = useFindCourseParticipants();
    const getLocalRoleHook = useGetLocalRoleByUserAtCourse();

    const { toNext } = useHistoryNavigation();

    useEffect(() => {
        if (kickModalActive || localRoleModalActive || inviteModalActive) return;
        fetchData();
    }, [user, localRole, pageParam, course, kickModalActive, localRoleModalActive, inviteModalActive]);

    async function fetchData() {
        await fetchLocalRole();
        await fetchParticipants();
    }

    async function fetchLocalRole() {
        if (localRole) {
            return;
        }

        resetValues();

        if (user && accessToken && course) {
            const localRole = await getLocalRoleHook.getLocalRoleQuery(course.id, user.id, accessToken);

            if (localRole) {
                setLocalRole(localRole);
            }
        }
    }

    async function fetchParticipants() {
        if (!localRole || !localRole.viewAccess) return;

        resetValues();

        if (user && accessToken && course) {
            const dataPage = await findCourseParticipantsHook.findParticipantsQuery(user.id, accessToken, course.id, page, searchText);

            if (dataPage) {
                setParticipants(dataPage.values);
                setPageCount(dataPage.pageCount);
            }
        }

    }

    async function onSubmit(event: React.FormEvent) {
        event.preventDefault();
        navigateToPage(1);
    }

    function onChange(event: React.ChangeEvent) {
        const inputElement = event.target as HTMLInputElement;
        setSearchText(inputElement.value);
    }

    function navigateToPage(nextPage: number) {
        setPage(nextPage);
        toNext(paths.course.participants.full(course?.id || 0, nextPage), true);
    }

    function resetValues() {
        getLocalRoleHook.resetValues();
        findCourseParticipantsHook.resetValues();
        setSelectedParticipant(null);
    }

    return (
        <>
            <MainContainer>
                {course ?
                    <>
                        <Breadcrumb>
                            <BreadcrumbItem text="В начало" path={paths.home} />
                            {!course.isPublic &&
                                <BreadcrumbItem text="Мои курсы" path={paths.profile.courses(ViewTypes.created)} />
                            }
                            <BreadcrumbItem text={course.title} path={paths.course.view.full(course.id)} />
                            <BreadcrumbItem text="Участники курса" />
                        </Breadcrumb>

                        <div className="line-distributed-container">
                            <h3>Участники курса</h3>
                            {localRole && localRole.inviteAccess &&
                                <ControlNav>
                                    <button className="control-nav__add-button button-gray icon icon-medium-size icon-plus"
                                        onClick={() => setInviteModalActive(true)}></button>
                                </ControlNav>
                            }
                        </div>


                        <SearchForm
                            placeholder="Найти по имени, фамилии, никнейму или локальной роли..."
                            onChange={onChange}
                            onSubmit={onSubmit}
                            value={searchText} />

                        <Paginate currentPage={page} pageCount={pageCount} setPage={navigateToPage} />



                        <BuildedParticipantsContainer
                            error={getLocalRoleHook.error || findCourseParticipantsHook.error}
                            onErrorModalClose={resetValues}
                            loading={getLocalRoleHook.loading || findCourseParticipantsHook.loading}>
                            {localRole && participants && participants.length > 0 ?
                                <section className="control-list">
                                    {participants.map(participant =>
                                        <ParticipantItem
                                            localRole={localRole}
                                            participant={participant}
                                            onEditButtonClick={() => {
                                                setSelectedParticipant(participant);
                                                setLocalRoleModalActive(true);
                                            }}
                                            onKickButtonClick={() => {
                                                setSelectedParticipant(participant);
                                                setKickModalActive(true);
                                            }}
                                        />
                                    )}
                                </section> :
                                <p>Участников пока нет...</p>
                            }

                            {user && localRole && localRole.inviteAccess &&
                                <ParticipantsInviteModal
                                    active={inviteModalActive}
                                    accessToken={accessToken}
                                    courseId={Number(courseParam.courseId)}
                                    onClose={() => setInviteModalActive(false)}
                                    userId={user.id}
                                />
                            }

                            {user && selectedParticipant &&
                                <>
                                    <ParticipantKickModal
                                        active={kickModalActive}
                                        onClose={() => setKickModalActive(false)}
                                        participant={selectedParticipant}
                                        accessToken={accessToken}
                                        courseId={Number(courseParam.courseId)}
                                        requesterUserId={user.id} />

                                    <ParticipantsLocalRoleModal
                                        participant={selectedParticipant}
                                        active={localRoleModalActive}
                                        accessToken={accessToken}
                                        courseId={Number(courseParam.courseId)}
                                        onClose={() => setLocalRoleModalActive(false)}
                                        userId={user.id}
                                    />
                                </>

                            }
                        </BuildedParticipantsContainer>
                    </> :
                    <p>Не удалось получить доступ к курсу...</p>
                }

            </MainContainer>

            <Outlet />
        </>

    );
}

interface IBuildedParticipantsContainer {
    error: string;
    onErrorModalClose: () => void;
    loading: boolean;
    children: React.ReactNode;
}

function BuildedParticipantsContainer({ error, onErrorModalClose, loading, children }: IBuildedParticipantsContainer) {
    if (loading) {
        return (<Loader />)
    }

    if (error) {
        return (<ErrorModal
            active={Boolean(error)}
            error={error}
            onClose={onErrorModalClose}
        />);
    }

    return children;
}

export default CourseParticipantsPage;