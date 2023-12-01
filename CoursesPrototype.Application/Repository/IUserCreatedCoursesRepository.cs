﻿using CoursesPrototype.Application.Repository.GenericRepository;
using CoursesPrototype.Core.Entities;

namespace CoursesPrototype.Application.Repository
{
    public interface IUserCreatedCoursesRepository : IAsyncWriteRepository<UserCreatedCourse>, IAsyncDisposable
    {
        Task<UserCreatedCourse[]> GetUserCreatedCoursesAsync(int userId);
        Task<UserCreatedCourse?> GetUserCreatedCourse(int userId, int courseId);
    }
}
