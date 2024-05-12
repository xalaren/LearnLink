import {Participant} from "../../models/participant.ts";
import profile from "../../assets/img/profile_placeholder.svg";
import {useEffect, useState} from "react";
import {LocalRole} from "../../models/localRole.ts";
import {useListAllLocalRolesAtCourse} from "../../hooks/courseLocalRoleHooks.ts";
import {useRequestReassignUserLocalRole} from "../../hooks/userCourseLocalRoleHooks.ts";
import {Modal} from "../../components/Modal/Modal.tsx";
import Select from "../../components/Select/Select.tsx";
import SelectItem from "../../components/Select/SelectItem.tsx";
import ModalFooter from "../../components/Modal/ModalFooter.tsx";
import ModalButton from "../../components/Modal/ModalButton.tsx";
import PopupNotification from "../../components/PopupNotification.tsx";
import {NotificationType} from "../../models/enums.ts";
import {useHistoryNavigation} from "../../hooks/historyNavigation.ts";
import {Paths} from "../../models/paths.ts";
import ModalContent from "../../components/Modal/ModalContent.tsx";

interface IParticipantsLocalRoleModalProps {
    participant: Participant;
    active: boolean;
    userId: number;
    accessToken: string;
    courseId: number;
    onClose: () => void;
}

function ParticipantsLocalRoleModal({
    participant,
    active,
    userId,
    accessToken,
    courseId,
    onClose}: IParticipantsLocalRoleModalProps) {
    const profileImage = participant.avatarUrl || profile;

    const [selectActive, setSelectActive] = useState(false);
    const [selectedLocalRole, setSelectedLocalRole] = useState<LocalRole | null>(participant.localRole);
    const [localRoles, setLocalRoles] = useState<LocalRole[]>();

    const getAtCourseHook = useListAllLocalRolesAtCourse();
    const requestReassignUserLocalRoleHook = useRequestReassignUserLocalRole();

    const {toNext} = useHistoryNavigation();

    useEffect(() => {
        fetchLocalRoles();
    }, []);

    async function fetchLocalRoles() {
        getAtCourseHook.resetValues();
        const result = await getAtCourseHook.listLocalRolesQuery(courseId, accessToken);

        if (result) {
            setLocalRoles(result);
        }
    }

    async function onSubmit() {
        if (selectedLocalRole == null) return;

        requestReassignUserLocalRoleHook.resetValues();
        await requestReassignUserLocalRoleHook.requestReassignUserLocalRoleQuery(userId, participant.id, courseId, selectedLocalRole.id, accessToken);
    }

    function resetDefaults() {
        getAtCourseHook.resetValues();
        requestReassignUserLocalRoleHook.resetValues();
        setSelectedLocalRole(participant.localRole);
    }

    function closeModal() {
        resetDefaults();
        onClose();
    }

    return (
        <>
            {!requestReassignUserLocalRoleHook.error && !requestReassignUserLocalRoleHook.loading && !requestReassignUserLocalRoleHook.success &&
                <Modal active={active} title="Редактировать роль пользователя" onClose={closeModal}>
                    <ModalContent className="indented">
                            <div className="user-item__profile">
                                <img className="user-item__image" src={profileImage} alt="Профиль" />
                                <div className="user-item__info profile-card">
                                    <p className="profile-card__title">
                                        {participant.name} {participant.lastname}
                                        <span className="profile-card__title text-violet"> (@{participant.nickname})</span>
                                    </p>
                                </div>
                            </div>

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
                                        onSelect={() => toNext(Paths.getCourseRolesPath(courseId))}
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

                            {getAtCourseHook.error &&
                                <p className="text-danger">{getAtCourseHook.error}</p>
                            }

                            {!getAtCourseHook.error && getAtCourseHook.loading &&
                                <p>Загрузка...</p>
                            }
                    </ModalContent>


                    <ModalFooter>
                        <ModalButton
                            text="Сохранить"
                            onClick={onSubmit}
                        />
                    </ModalFooter>
                </Modal >
            }

            {requestReassignUserLocalRoleHook.error &&
                <PopupNotification notificationType={NotificationType.error} onFade={closeModal}>
                    {requestReassignUserLocalRoleHook.error}
                </PopupNotification>
            }

            {requestReassignUserLocalRoleHook.success &&
                <PopupNotification notificationType={NotificationType.success} onFade={closeModal}>
                    {requestReassignUserLocalRoleHook.success}
                </PopupNotification>
            }
        </>
    );
}

export default ParticipantsLocalRoleModal;