namespace UnitTests.Persistence.Helpers
{
    using global::Persistence.Interfaces;
    using global::Persistence.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Threading;
    using System.Threading.Tasks;

    public class FakeUsersDbContext : IUsersDbContext
    {
        public FakeUsersDbContext()
        {
            this.Users = new FakeUserSet();
        }

        public DbSet<User> Users { get; private set; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => Task.FromResult(0);
    }
}
