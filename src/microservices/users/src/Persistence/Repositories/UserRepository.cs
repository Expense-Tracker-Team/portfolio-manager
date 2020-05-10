namespace Persistence.Repositories
{
    using Application.Infrastructure.Repositories;
    using Domain;
    using Microsoft.EntityFrameworkCore;
    using Persistence.Interfaces;
    using System;
    using System.Threading.Tasks;

    using UserDataModel = Persistence.Models.User;

    public class UserRepository : IUserRepository
    {
        private readonly UsersDbContext dbContext;

        public UserRepository(IUsersDbContext dbContext) => this.dbContext = (UsersDbContext)dbContext;

        public async Task<User> CreateAsync(User user)
        {
            var dataModel = new UserDataModel
            {
                Id = user.Id,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };

            this.dbContext.Users.Add(dataModel);
            await this.dbContext.SaveChangesAsync();

            return new User(dataModel.Id, dataModel.Email, dataModel.PasswordHash, dataModel.Name, dataModel.PhoneNumber);
        }

        public async Task<User> Get(Guid userId)
        {
            var user = await this.dbContext.Users.AsNoTracking().SingleOrDefaultAsync(x => x.Id == userId);

            return new User(user.Id, user.Email, user.PasswordHash, user.Name, user.PhoneNumber);
        }
    }
}