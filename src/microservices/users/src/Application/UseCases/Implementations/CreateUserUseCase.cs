namespace Application.UseCases.Implementations
{
    using Application.Infrastructure.Repositories;
    using Application.UseCases.Interfaces;
    using Application.UseCases.Models;
    using Domain;
    using System;
    using System.Threading.Tasks;

    public class CreateUserUseCase : ICreateUserUseCase
    {
        private readonly IUserRepository repository;

        public CreateUserUseCase(IUserRepository repository) => this.repository = repository;

        public async Task<User> ExecuteAsync(CreateUserInput createUser)
        {
            var passwordHash = createUser.Password + "_hashed";            
            var user = new User(Guid.NewGuid(), createUser.Email, passwordHash, createUser.Name, createUser.PhoneNumber);

            User createdUser = await this.repository.Create(user);
            
            return createdUser;
        }
    }
}
