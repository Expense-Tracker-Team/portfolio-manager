namespace Persistence.Repositories
{
    using Application.Infrastructure.Repositories;
    using Domain;
    using System.Threading.Tasks;

    using UserDataModel = Persistence.Models.User;

    public class UserRepository : IUserRepository
    {
        private readonly UsersDbContext dbContext;

        public UserRepository(UsersDbContext dbContext) => this.dbContext = dbContext;

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
    }
}