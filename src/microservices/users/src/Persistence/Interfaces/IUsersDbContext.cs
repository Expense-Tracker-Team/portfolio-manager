﻿namespace Persistence.Interfaces
{
    using Microsoft.EntityFrameworkCore;
    using Persistence.Models;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IUsersDbContext
    {
        DbSet<UserDataModel> Users { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}