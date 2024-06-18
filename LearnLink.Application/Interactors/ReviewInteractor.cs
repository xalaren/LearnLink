using LearnLink.Application.Mappers;
using LearnLink.Application.Transaction;
using LearnLink.Core.Entities;
using LearnLink.Core.Exceptions;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Application.Interactors
{
    public class ReviewInteractor(IUnitOfWork unitOfWork)
    {

        public async Task<Response<ReviewDto?>> GetReviewByAnswerAsync(int answerId)
        {
            try
            {
                var answerReview = await unitOfWork.AnswerReviews
                    .FirstOrDefaultAsync(answerReview => answerReview.AnswerId == answerId);

                if (answerReview == null)
                {
                    return new()
                    {
                        Success = true,
                        StatusCode = 200,
                        Message = "Оценка получена успешно",
                        Value = null
                    };
                }

                await unitOfWork.AnswerReviews.Entry(answerReview)
                    .Reference(answerReview => answerReview.Review)
                    .LoadAsync();

                await unitOfWork.Reviews.Entry(answerReview.Review)
                    .Reference(review => review.ExpertUser)
                    .LoadAsync();


                return new()
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Оценка получена успешно",
                    Value = answerReview.Review.ToDto()
                };
            }
            catch (CustomException exception)
            {
                return new()
                {
                    Success = false,
                    StatusCode = exception.StatusCode,
                    Message = exception.Message,
                };
            }
            catch (Exception exception)
            {
                return new()
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Не удалось получить оценку",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task<Response> CreateReviewAsync(ReviewDto reviewDto, int answerId)
        {
            try
            {
                var existingAnswerReview = await unitOfWork.AnswerReviews
                     .FirstOrDefaultAsync(answerReview => answerReview.AnswerId == answerId);

                if (existingAnswerReview != null)
                {
                    throw new ValidationException("Оценка на данный ответ уже записана");
                }

                var review = reviewDto.ToEntity();

                await unitOfWork.Reviews.AddAsync(review);
                await unitOfWork.CommitAsync();

                var answerReview = new AnswerReview()
                {
                    AnswerId = answerId,
                    ReviewId = review.Id,
                };

                await unitOfWork.AnswerReviews.AddAsync(answerReview);
                await unitOfWork.CommitAsync();

                return new()
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Оценка записана успешно",
                };
            }
            catch (CustomException exception)
            {
                return new()
                {
                    Success = false,
                    StatusCode = exception.StatusCode,
                    Message = exception.Message,
                };
            }
            catch (Exception exception)
            {
                return new()
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Не удалось записать оценку",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task<Response> UpdateReviewAsync(ReviewDto reviewDto)
        {
            try
            {
                var review = await unitOfWork.Reviews.FindAsync(reviewDto.Id);

                NotFoundException.ThrowIfNotFound(review, "Оценка к заданию не найдена");

                review = review.Assign(reviewDto);

                unitOfWork.Reviews.Update(review);
                await unitOfWork.CommitAsync();

                return new()
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Оценка записана успешно",
                };
            }
            catch (CustomException exception)
            {
                return new()
                {
                    Success = false,
                    StatusCode = exception.StatusCode,
                    Message = exception.Message,
                };
            }
            catch (Exception exception)
            {
                return new()
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Не удалось записать оценку",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task<Response> RemoveReviewAsync(int reviewId)
        {
            try
            {
                var review = await unitOfWork.Reviews.FindAsync(reviewId);

                NotFoundException.ThrowIfNotFound(review, "Оценка к заданию не найдена");

                unitOfWork.Reviews.Remove(review);
                await unitOfWork.CommitAsync();

                return new()
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Оценка записана успешно",
                };
            }
            catch (CustomException exception)
            {
                return new()
                {
                    Success = false,
                    StatusCode = exception.StatusCode,
                    Message = exception.Message,
                };
            }
            catch (Exception exception)
            {
                return new()
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Не удалось записать оценку",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task RemoveReviewByAnswerAsyncNoResponse(int answerId)
        {
            var answerReview = await unitOfWork.AnswerReviews.FirstOrDefaultAsync(answerReview => answerReview.AnswerId == answerId);

            NotFoundException.ThrowIfNotFound(answerReview, "Ответ к заданию не найден");

            await unitOfWork.AnswerReviews.Entry(answerReview)
                .Reference(answerReview => answerReview.Review)
                .LoadAsync();

            unitOfWork.Reviews.Remove(answerReview.Review);
            unitOfWork.AnswerReviews.Remove(answerReview);
        }
    }
}
