namespace Application.UseCases.Interfaces
{
    using Application.UseCases.Models;
    using Domain;
    using System.Threading.Tasks;

    public interface ICreateUserUseCase
    {
        Task<User> ExecuteAsync(CreateUserInput createUser);
    }
}
