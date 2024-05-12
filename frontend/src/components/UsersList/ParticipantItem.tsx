import { LocalRole } from "../../models/localRole";
import profile from '../../assets/img/profile_placeholder.svg';
import { Participant } from "../../models/participant";

interface IUserItemProps {
    participant: Participant;
    localRole: LocalRole;
    onEditButtonClick: () => void;
    onKickButtonClick: () => void;
    className?: string;
}

function ParticipantItem({
    participant,
    localRole,
    onEditButtonClick,
    onKickButtonClick,
    className = '' }: IUserItemProps) {

    const profileImage = participant.avatarUrl || profile;

    return (
        <div className={`user-item ${className}`}>
            <div className="user-item__profile">
                <img className="user-item__image" src={profileImage} alt="Профиль" />
                <div className="user-item__info profile-card">
                    <p className="profile-card__title">{participant.name} {participant.lastname}</p>
                    <p className="profile-card__subtitle text-violet">@{participant.nickname}</p>
                    <p className="profile-card__subtitle">Роль: <span className="text-violet">{participant.localRole.name}</span></p>
                </div>
            </div>
            <div className="user-item__properties">
                {localRole.inviteAccess &&
                    <button
                        className="user-item__button button-violet-light-rounded icon-pen icon-medium-size"
                        onClick={onEditButtonClick}>
                    </button>
                }
                {localRole.kickAccess &&
                    <button
                        className="user-item__button button-violet-light-rounded icon-cross icon-medium-size"
                        onClick={onKickButtonClick}>
                    </button>
                }
            </div>
        </div >
    );
}

export default ParticipantItem;