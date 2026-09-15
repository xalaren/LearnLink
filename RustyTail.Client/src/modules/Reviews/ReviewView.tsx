import { useEffect, useState } from "react";
import { useAppSelector } from "../../hooks/redux";
import { useReviewQueries } from "../../hooks/reviewHooks";

import { Loader } from "../../components/Loader/Loader";
import { Review } from "../../models/review";
import GradeView from "../../components/GradeView";
import CommentBlock from "../../components/CommentBlock";
import { LocalRole } from "../../models/localRole";
import EditReviewModal from "./EditReviewModal";

interface ReviewViewProps {
    answerId: number;
    localRole: LocalRole;
}

function ReviewView({ answerId, localRole }: ReviewViewProps) {
    const { getReviewByAnswerQuery, loading } = useReviewQueries();
    const { accessToken } = useAppSelector(state => state.authReducer);
    const [review, setReview] = useState<Review | null>();
    const [editReviewModalActive, setEditReviewModalActive] = useState(false);

    useEffect(() => {
        if (!accessToken || editReviewModalActive) return;
        fetchReview();

    }, [accessToken, editReviewModalActive])

    async function fetchReview() {
        const result = await getReviewByAnswerQuery(answerId, accessToken);
        setReview(result);
    }


    return (
        <>
            {review &&
                <section className="review">
                    {!loading ?
                        <>
                            <div className="line-distributed-container">
                                <p className="ui-title review__title">
                                    Оценка за задание:
                                </p>
                                {localRole.manageInternalAccess &&
                                    <button
                                        className="button-gray-violet"
                                        onClick={() => setEditReviewModalActive(true)}>
                                        Редактировать
                                    </button>
                                }


                            </div>


                            <GradeView currentValue={review.grade} maxValue={5} />

                            <CommentBlock
                                userName={`${review.expertUserDetails?.lastname || ''} ${review.expertUserDetails?.name || ''}`}
                                comment={review.comment || 'Оценено без комментария'}
                                date={review.reviewDate}
                                avatarUrl={review.expertUserDetails?.avatarUrl}
                            />

                            <EditReviewModal
                                active={editReviewModalActive}
                                review={review}
                                onClose={() => setEditReviewModalActive(false)}
                            />

                        </> :
                        <Loader />
                    }

                </section>
            }
        </>
    );
}

export default ReviewView;