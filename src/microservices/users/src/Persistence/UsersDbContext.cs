namespace Persistence
{
    using Microsoft.EntityFrameworkCore;
    using Persistence.EntityTypeConfigurations;
    using Persistence.Interfaces;
    using Persistence.Models;

    public class UsersDbContext : DbContext, IUsersDbContext
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
    }
}
