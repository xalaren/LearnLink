import { useParams } from "react-router-dom";
import { MainContainer } from "../components/MainContainer";
import SearchForm from "../components/SearchForm";
import React, { useEffect, useState } from "react";
import { useHistoryNavigation } from "../hooks/historyNavigation";
import { Paths } from "../models/paths";
import Paginate from "../components/Paginate";
import ControlNav from "../components/ControlNav";
import { LocalRole } from "../models/localRole";
import { useAppSelector } from "../hooks/redux";
import { useFindCourseParticipants } from "../hooks/courseHooks";
import { Participant } from "../models/participant";
import { Loader } from "../components/Loader/Loader";
import { ErrorModal } from "../components/Modal/ErrorModal";
import ParticipantItem from "../components/UsersList/ParticipantItem";
import { useGetLocalRoleByUserAtCourse } from "../hooks/userCourseLocalRoleHooks.ts";
import ParticipantsLocalRoleModal from "../modules/Participants/ParticipantsLocalRoleModal.tsx";
import ParticipantKickModal from "../modules/Participants/ParticipantKickModal.tsx";
import ParticipantsInviteModal from "../modules/Participants/ParticipantsInviteModal.tsx";

function CourseParticipantsPage() {
    const courseParam = useParams<'courseId'>();
    const pageParam = useParams<'pageNumber'>();

    const [page, setPage] = useState(Number(pageParam.pageNumber));
    const [pageCount, setPageCount] = useState(1);
    const [searchText, setSearchText] = useState('');
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
    }, [user, localRole, pageParam, kickModalActive, localRoleModalActive, inviteModalActive]);

    async function fetchData() {
        await fetchLocalRole();
        await fetchParticipants();
    }

    async function fetchLocalRole() {
        if (localRole) {
            return;
        }

        resetValues();

        if (user && accessToken) {
            const localRole = await getLocalRoleHook.getLocalRoleQuery(Number(courseParam.courseId), user.id, accessToken);

            if (localRole) {
                setLocalRole(localRole);
            }
        }
    }

    async function fetchParticipants() {
        if (!localRole || !localRole.viewAccess) return;

        resetValues();

        if (user && accessToken) {
            const dataPage = await findCourseParticipantsHook.findParticipantsQuery(user.id, accessToken, Number(courseParam.courseId), page, searchText);

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
        toNext(`${Paths.getCourseParticipantsPath(Number(courseParam.courseId))}/${nextPage}`);
    }

    function resetValues() {
        getLocalRoleHook.resetValues();
        findCourseParticipantsHook.resetValues();
        setSelectedParticipant(null);
    }

    return (
        <MainContainer title="Участники курса">
            <SearchForm
                placeholder="Найти по имени, фамилии или никнейму..."
                onChange={onChange}
                onSubmit={onSubmit}
                value={searchText} />

            <Paginate currentPage={page} pageCount={pageCount} setPage={navigateToPage} />

            {localRole && localRole.inviteAccess &&
                <div className="line-end-container">
                    <ControlNav>
                        <button className="control-nav__add-button button-gray icon-plus"
                            onClick={() => setInviteModalActive(true)}></button>
                    </ControlNav>
                </div>
            }

            <BuildedParticipantsContainer
                error={getLocalRoleHook.error || findCourseParticipantsHook.error}
                onErrorModalClose={resetValues}
                loading={getLocalRoleHook.loading || findCourseParticipantsHook.loading}>
                {localRole && participants &&
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
                    </section>
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


        </MainContainer >
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