import { Answer } from "../../models/answer";
import profile from '../../assets/img/profile_placeholder.svg';

interface AnswerItemProps {
    answer: Answer;
    showReviewButton: boolean;
    showViewButton: boolean;
    onReviewClick: () => void;
    onViewClick: () => void;
}

function AnswerItem({ answer, showReviewButton, showViewButton, onReviewClick, onViewClick }: AnswerItemProps) {
    const profileImage = answer.userDetails?.avatarUrl || profile;

    return (
        <div className="response-list__list-item user-item">
            <div className="user-item__profile">
                <img className="user-item__image" src={profileImage} alt="Профиль" />
                <div className="user-item__info profile-card">
                    {answer.userDetails &&
                        <>
                            <p className="profile-card__title">{answer.userDetails.name} {answer.userDetails.lastname}</p>
                            <p className="profile-card__subtitle text-violet">@{answer.userDetails.nickname}</p>
                        </>
                    }
                    <p className="profile-card__subtitle">Дата выполнения:&nbsp;
                        <span className="text-violet">{answer.uploadDate}</span>
                    </p>
                    <p className="profile-card__subtitle">Оценка:&nbsp;
                        {answer.grade ?
                            <span className="text-violet">{answer.grade}</span> :
                            <span className="text-violet">нет оценки</span>
                        }
                    </p>
                </div>
            </div>
            <div className="user-item__properties">
                <div className="user-item__buttons">
                    {showViewButton &&
                        <button className="button-gray-violet" onClick={onViewClick}>
                            <span className="icon-arrow-right icon-medium-size"></span>
                            Просмотреть
                        </button>
                    }
                    {showReviewButton &&
                        <button className="button-gray-violet" onClick={onReviewClick}>
                            <span className="icon-star icon-medium-size"></span>
                            Оценить
                        </button>
                    }
                </div>
            </div>
        </div>
    );
}

export default AnswerItem;