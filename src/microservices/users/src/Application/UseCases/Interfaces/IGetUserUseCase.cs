namespace Application.UseCases.Interfaces
{
    using Domain;
    using System;
    using System.Threading.Tasks;

    public interface IGetUserUseCase
    {
        Task<User> ExecuteAsync(Guid userId);
    }
}
