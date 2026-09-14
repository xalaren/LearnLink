import profile from '../assets/img/profile_placeholder.svg';
import { InviteItem } from "../models/inviteItem";

interface IInviteListProps {
    children: InviteItem[];
}

function InviteList({ children }: IInviteListProps) {
    return (
        <div className="invite-list">
            {children.map(item =>
                <InviteListItem
                    key={item.user.id}
                    name={item.user.name}
                    lastname={item.user.lastname}
                    nickname={item.user.nickname}
                    onInvite={item.onInvite}
                    avatar={item.user.avatarUrl}
                />
            )}
        </div>
    );
}

interface IInviteListItemProps {
    name: string;
    lastname: string;
    nickname: string;
    avatar?: string;
    onInvite: () => void;
}

function InviteListItem({ name, lastname, nickname, onInvite, avatar }: IInviteListItemProps) {
    const avatarSource: string = avatar || profile;

    return (
        <div className="invite-list__item">
            <div className="invite-list__user user-mini">
                <img src={avatarSource} alt="" className="user-mini__image" />
                <div className="user-mini__card">
                    <div className="user-mini__name">{name} {lastname}</div>
                    <div className="user-mini__nickname">@{nickname}</div>
                </div>
            </div>
            <button
                onClick={onInvite}
                className="invite-list__button button-violet-light-rounded icon icon-medium-size icon-plus"></button>
        </div>);
}

export default InviteList;