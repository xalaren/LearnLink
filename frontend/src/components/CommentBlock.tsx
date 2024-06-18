import profile from '../assets/img/profile_placeholder.svg';

interface CommentBlockProps {
    userName: string;
    comment?: string;
    date: string;
    avatarUrl?: string;
}


function CommentBlock({ userName, date, comment, avatarUrl = profile }: CommentBlockProps) {
    return (
        <div className="comment">
            <div className="comment__head">
                <div className="comment__avatar">
                    <img className="avatar-image" src={avatarUrl} alt="Профиль" />
                </div>
                <div className="comment__user-info">
                    <p>{userName}</p>
                </div>
            </div>
            {comment ?
                <div className="comment__body">{comment}</div> :
                <div className="comment__body comment__body-none">Комментарий не был добавлен...</div>
            }
            <div className="comment__footer">{date}</div>
        </div>
    );
}

export default CommentBlock;