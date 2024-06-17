using LearnLink.Application.Helpers;
using LearnLink.Core.Entities;
using LearnLink.Shared.DataTransferObjects;

namespace LearnLink.Application.Mappers
{
    public static class ReviewMapper
    {
        public static Review ToEntity(this ReviewDto reviewDto)
        {
            return new Review()
            {
                Id = reviewDto.Id,
                Grade = reviewDto.Grade,
                ExpertUserId = reviewDto.ExpertUserId,
                Comment = reviewDto.Comment,
                ReviewDate = DateTime.UtcNow
            };
        }

        public static ReviewDto ToDto(this Review reviewEntity)
        {
            return new ReviewDto()
            {
                Id = reviewEntity.Id,
                Grade = reviewEntity.Grade,
                ExpertUserId = reviewEntity.ExpertUserId,
                ExpertUserDetails = new UserLiteDetailsDto()
                {
                    Id = reviewEntity.ExpertUser.Id,
                    Name = reviewEntity.ExpertUser.Name,
                    Lastname = reviewEntity.ExpertUser.Lastname,
                    Nickname = reviewEntity.ExpertUser.Nickname,
                    AvatarUrl = reviewEntity.ExpertUser.AvatarFileName != null ?
                    DirectoryStore.GetRelativeDirectoryUrlToUserImages(reviewEntity.ExpertUser.Id) + reviewEntity.ExpertUser.AvatarFileName : null
                },
                Comment = reviewEntity.Comment,
                ReviewDate = reviewEntity.ReviewDate.ToLocalDateTime().ToString()
            };
        }

        public static Review Assign(this Review reviewEntity, ReviewDto reviewDto)
        {
            reviewEntity.Grade = reviewDto.Grade;
            reviewEntity.Comment = reviewDto.Comment;
            reviewEntity.ReviewDate = DateTime.UtcNow;

            return reviewEntity;
        }
    }
}
