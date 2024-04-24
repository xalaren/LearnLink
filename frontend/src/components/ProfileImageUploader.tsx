import defaultImage from '../assets/img/profile_placeholder.svg';

interface IProfileImageUploaderProps {
    name: string;
    onChange: (event: React.ChangeEvent) => void
    className?: string;
    image?: File;
    userImage?: string;
}

function ProfileImageUploader({ name, onChange, className = '', image, userImage }: IProfileImageUploaderProps) {
    let imageUrl = '';

    if (image) {
        imageUrl = URL.createObjectURL(image);
    }
    else if (userImage) {
        imageUrl = userImage;
    }
    else {
        imageUrl = defaultImage;
    }

    return (
        <div className={`image-uploader ${className}`}>
            <label htmlFor="image-upload" className="image-uploader__label">
                <div className="image-reuploadable">
                    <img className='avatar-image' src={imageUrl} alt="Изображение профиля" />
                    <div className="image-reuploadable__icon icon-update"></div>
                </div>
            </label>
            <input
                type="file"
                className="image-uploader__input"
                name={name}
                id="image-upload"
                accept="image/jpeg, image/png, image/jpg"
                onChange={onChange} />
        </div >
    );
}

export default ProfileImageUploader;