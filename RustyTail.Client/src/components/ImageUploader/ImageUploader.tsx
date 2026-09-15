import defaultImage from '../../assets/img/profile_add.svg';

interface IImageUploaderProps {
    name: string;
    onChange: (event: React.ChangeEvent) => void
    className?: string;
    image?: File;
}

function ImageUploader({ name, onChange, className = '', image }: IImageUploaderProps) {
    let imageUrl = '';

    if (image) {
        imageUrl = URL.createObjectURL(image);
    }
    else {
        imageUrl = defaultImage;
    }

    return (
        <div className={`image-uploader ${className}`}>
            <label htmlFor="image-upload" className="image-uploader__label">
                <img className='avatar-image' src={imageUrl} alt="Изображение профиля" />
                <p>{image && image.name}</p>
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

export default ImageUploader;