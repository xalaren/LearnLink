﻿using CoursesPrototype.Application.Repository.GenericRepository;
using CoursesPrototype.Core.Entities;

namespace CoursesPrototype.Application.Repository
{
    public interface ICredentialsRepository : IAsyncReadRepository<Credentials>, IAsyncWriteRepository<Credentials>, IUpdateRepository<Credentials>, IAsyncDisposable
    {
        Task<Credentials?> GetCredentialsByUserId(int userId);
    }
}
