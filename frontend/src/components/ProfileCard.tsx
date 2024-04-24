import { User } from "../models/user";
import ProfileImageUploader from "./ProfileImageUploader";

interface IProfileCardProps {
    user: User;
    inputName: string;
    className?: string;
    onImageChange: (event: React.ChangeEvent) => void
    profileImage?: File;
}

function ProfileCard({ user, inputName, onImageChange, profileImage, className = '' }: IProfileCardProps) {
    return (
        <div className={`profile-card ${className}`}>
            <ProfileImageUploader
                className="profile-card__image"
                name={inputName}
                onChange={onImageChange}
                image={profileImage}
                userImage={user.avatarUrl}
            />
            <p className="profile-card__title">{user.name} {user.lastname}</p>
            <p className="profile-card__subtitle text-violet">@{user.nickname} ({user.role?.name})</p>
        </div >
    );
}

export default ProfileCard;