import { Modal } from "../../components/Modal/Modal.tsx";
import Paginate from "../../components/Paginate.tsx";
import React, { useEffect, useState } from "react";
import SearchForm from "../../components/SearchForm.tsx";
import { User } from "../../models/user.ts";
import InviteList from "../../components/InviteList.tsx";
import { useFindUsers } from "../../hooks/userHooks.ts";
import { Loader } from "../../components/Loader/Loader.tsx";
import { InviteItem } from "../../models/inviteItem.ts";
import ModalFooter from "../../components/Modal/ModalFooter.tsx";
import { useInvite } from "../../hooks/subscriptionHooks.ts";
import { LocalRole } from "../../models/localRole.ts";
import { useListAllLocalRolesAtCourse } from "../../hooks/courseLocalRoleHooks.ts";
import Select from "../../components/Select/Select.tsx";
import SelectItem from "../../components/Select/SelectItem.tsx";
import PopupNotification from "../../components/PopupNotification.tsx";
import { NotificationType } from "../../models/enums.ts";
import { useHistoryNavigation } from "../../hooks/historyNavigation.ts";
import { paths } from "../../models/paths.ts";
import ModalContent from "../../components/Modal/ModalContent.tsx";


interface IParticipantsInviteModalProps {
    active: boolean;
    userId: number;
    accessToken: string;
    courseId: number;
    onClose: () => void;
}

function ParticipantsInviteModal(
    { active, userId, accessToken, courseId, onClose }: IParticipantsInviteModalProps
) {
    const pageSize: number = 6;
    const [page, setPage] = useState(1);
    const [pageCount, setPageCount] = useState(1);
    const [searchText, setSearchText] = useState('');
    const [foundUsers, setFoundUsers] = useState<User[]>();
    const [selectActive, setSelectActive] = useState(false);
    const [selectedLocalRole, setSelectedLocalRole] = useState<LocalRole | null>();
    const [localRoles, setLocalRoles] = useState<LocalRole[]>();

    const { toNext } = useHistoryNavigation();

    const findUsersHook = useFindUsers();
    const inviteUserHook = useInvite();
    const getAtCourseHook = useListAllLocalRolesAtCourse();

    useEffect(() => {
        fetchData();
    }, [inviteUserHook.success])

    async function fetchData() {
        await fetchUsers();

        if (selectedLocalRole) return;
        await fetchLocalRoles();
    }

    async function fetchLocalRoles() {
        getAtCourseHook.resetValues();
        const result = await getAtCourseHook.listLocalRolesQuery(courseId, accessToken);

        if (result) {
            setLocalRoles(result);
        }
    }

    async function fetchUsers() {
        const dataPage = await findUsersHook.findUsersQuery(courseId, page, pageSize, accessToken, searchText);

        if (dataPage) {
            setFoundUsers(dataPage.values);
            setPageCount(dataPage.pageCount);
        }
    }

    function onChange(event: React.ChangeEvent) {
        const inputElement = event.target as HTMLInputElement;
        setSearchText(inputElement.value);
    }

    async function onInvite(inviteUserId: number) {
        if (selectedLocalRole == null) return;
        await inviteUserHook.inviteQuery(userId, accessToken, courseId, selectedLocalRole.id, inviteUserId);
    }

    async function onSearchFormSubmit(event: React.FormEvent) {
        event.preventDefault();
        await fetchUsers();
    }


    function closeModal() {
        resetDefaults();
        onClose();
    }

    function resetDefaults() {
        findUsersHook.resetValues();
        getAtCourseHook.resetValues();
        inviteUserHook.resetValues();
    }



    return (
        <>
            <Modal active={active} onClose={closeModal} title="Приглашение участников">
                <ModalContent className="indented">
                    {!getAtCourseHook.error && !getAtCourseHook.loading &&
                        <Select
                            active={selectActive}
                            onDeselect={() => setSelectActive(false)}
                            toggle={() => setSelectActive(prev => !prev)}
                            defaultTitle="Выберите локальную роль из списка..."
                            selectedTitle={selectedLocalRole?.name}>

                            <SelectItem
                                key={0}
                                title="Добавить локальные роли..."
                                onSelect={() => toNext(paths.course.roles.full(courseId))}
                            />
                            {localRoles && localRoles.map(localRole =>

                                <SelectItem
                                    key={localRole.id}
                                    title={localRole.name}
                                    onSelect={() => {
                                        setSelectedLocalRole(localRole);
                                        setSelectActive(false);
                                    }}
                                />
                            )}

                        </Select>
                    }

                    <SearchForm
                        placeholder="Найти по имени, фамилии или никнейму..."
                        onChange={onChange}
                        onSubmit={onSearchFormSubmit}
                        value={searchText} />

                    <Paginate currentPage={page} pageCount={pageCount} setPage={setPage} />

                    {!findUsersHook.error && !findUsersHook.loading && foundUsers &&
                        <InviteList>
                            {foundUsers.map<InviteItem>(item => new InviteItem(item, () => onInvite(item.id)))}
                        </InviteList>
                    }

                    {foundUsers?.length == 0 &&
                        <p>Нет доступных пользователей</p>
                    }

                    {findUsersHook.error || getAtCourseHook.error &&
                        <p className="text-danger">{findUsersHook.error || getAtCourseHook.error}</p>
                    }

                    {!(findUsersHook.error || getAtCourseHook.error) && findUsersHook.loading || getAtCourseHook.loading &&
                        <Loader />
                    }
                </ModalContent>

                <ModalFooter>
                    <></>
                </ModalFooter>
            </Modal>

            {inviteUserHook.error &&
                <PopupNotification notificationType={NotificationType.error} onFade={resetDefaults}>
                    {inviteUserHook.error}
                </PopupNotification>
            }

            {inviteUserHook.success &&
                <PopupNotification notificationType={NotificationType.success} onFade={resetDefaults}>
                    {inviteUserHook.success}
                </PopupNotification>
            }
        </>
    );

}

export default ParticipantsInviteModal;