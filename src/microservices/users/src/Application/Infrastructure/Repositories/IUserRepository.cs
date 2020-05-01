namespace Application.Infrastructure.Repositories
{
    using Domain;
    using System;
    using System.Threading.Tasks;

    public interface IUserRepository
    {
        Task<User> Create(User user);

        Task<User> Get(Guid userId);
    }
}
