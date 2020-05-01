namespace Application.UseCases.Implementations
{
    using Application.Infrastructure.Repositories;
    using Application.UseCases.Interfaces;
    using Domain;
    using System;
    using System.Threading.Tasks;

    public class GetUserUseCase : IGetUserUseCase
    {
        private readonly IUserRepository repository;

        public GetUserUseCase(IUserRepository repository) => this.repository = repository;

        public async Task<User> ExecuteAsync(Guid userId) => await this.repository.Get(userId);
    }
}
