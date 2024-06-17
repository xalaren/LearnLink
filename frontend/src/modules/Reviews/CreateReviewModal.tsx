import { useEffect, useState } from "react";
import { Input } from "../../components/Input/Input";
import { Modal } from "../../components/Modal/Modal";
import ModalButton from "../../components/Modal/ModalButton";
import ModalContent from "../../components/Modal/ModalContent";
import ModalFooter from "../../components/Modal/ModalFooter";
import { InputType, NotificationType } from "../../models/enums";
import RateSelector from "../../components/RateSelector";
import { useReviewQueries } from "../../hooks/reviewHooks";
import PopupLoader from "../../components/Loader/PopupLoader";
import PopupNotification from "../../components/PopupNotification";
import { useAppSelector } from "../../hooks/redux";
import { Review } from "../../models/review";

interface CreateReviewModalProps {
    active: boolean;
    onClose: () => void;
    answerId: number;
}


function CreateReviewModal({ answerId, active, onClose }: CreateReviewModalProps) {
    const [comment, setComment] = useState('');

    const [rateError, setRateError] = useState('');
    const [rateValue, setRateValue] = useState(0);

    const { accessToken } = useAppSelector(state => state.authReducer);
    const { user } = useAppSelector(state => state.userReducer);

    const { createQuery, error, success, loading, resetValues } = useReviewQueries();

    async function createReview() {
        if (rateValue === 0) {
            setRateError("Оценка не была выбрана!");
            return;
        }

        if (!user || !accessToken) return;

        const review: Review = {
            id: 0,
            grade: rateValue,
            comment: comment,
            reviewDate: Date.now.toString(),
            expertUserId: user.id,
            expertUserDetails: {
                id: user.id,
                nickname: user.nickname,
                lastname: user.lastname,
                name: user.name,
                avatarUrl: user.avatarUrl
            }
        };

        await createQuery(answerId, review, accessToken);

    }

    function changeRateValue(value: number) {
        setRateError('');
        setRateValue(value);
    }

    function closeModal() {
        setRateError('');
        setComment('');
        setRateValue(0);
        resetValues();
        onClose();
    }


    return (
        <>
            {!loading && !error && !success &&
                <Modal
                    active={active}
                    onClose={closeModal}
                    title="Оценка ответа на задание">

                    <ModalContent>

                        <form className="form">
                            <div className="form__inputs">
                                <div className="form-input">
                                    {!rateError &&
                                        <p className={`form-input__label form-input__label-required`}>Оценка за задание:</p>
                                    }
                                    {rateError &&
                                        <p className="form-input__error required">{rateError}</p>
                                    }
                                    <RateSelector
                                        minRate={1}
                                        maxRate={5}
                                        selectedRateValue={rateValue}
                                        setSelectedRateValue={changeRateValue}
                                    />

                                </div>
                                <Input
                                    type={InputType.rich}
                                    name="comment"
                                    label="Комментарий"
                                    placeholder="Введите комментарий (необязательно)..."
                                    value={comment}
                                    onChange={(event) => setComment((event.target as HTMLInputElement).value)}
                                />
                            </div>
                        </form>
                    </ModalContent>

                    <ModalFooter>
                        <ModalButton text="Сохранить" onClick={createReview} />
                    </ModalFooter>

                </Modal >
            }

            {loading &&
                <PopupLoader />
            }

            {success &&
                <PopupNotification notificationType={NotificationType.success} onFade={closeModal}>
                    {success}
                </PopupNotification>
            }

            {error &&
                <PopupNotification notificationType={NotificationType.error} onFade={closeModal}>
                    {error}
                </PopupNotification>
            }
        </>
    );
}

export default CreateReviewModal;