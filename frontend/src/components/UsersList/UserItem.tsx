import { LocalRole } from "../../models/localRole";
import { User } from "../../models/user";
import profile from '../../assets/img/profile_placeholder.svg';

interface IUserItemProps {
    user: User;
    localRole: LocalRole;
    onEditButtonClick: () => void;
    onKickButtonClick: () => void;
    className?: string;
}

function UserItem({
    user,
    localRole,
    onEditButtonClick,
    onKickButtonClick,
    className = '' }: IUserItemProps) {

    let profileImage = user.avatarUrl || profile;

    return (
        <div className={`user-item ${className}`}>
            <div className="user-item__profile">
                <img className="user-item__image" src={profileImage} alt="Профиль" />
                <div className="user-item__info profile-card">
                    <p className="profile-card__title">{user.name} {user.lastname}</p>
                    <p className="profile-card__subtitle text-violet">@{user.nickname}</p>
                    <p className="profile-card__subtitle">Роль: <span className="text-violet">{localRole.name}</span></p>
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
        </div>
    );
}

export default UserItem;