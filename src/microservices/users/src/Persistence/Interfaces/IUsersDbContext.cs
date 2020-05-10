namespace Persistence.Interfaces
{
    using Microsoft.EntityFrameworkCore;
    using Persistence.Models;

    public interface IUsersDbContext
    {
        DbSet<User> Users { get; }
    }
}