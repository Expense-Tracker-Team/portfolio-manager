namespace Api
{
    using System.Threading.Tasks;
    using Api.Protos;
    using Application.UseCases.Interfaces;
    using Application.UseCases.Models;
    using Grpc.Core;
    using Microsoft.Extensions.Logging;

    public class UserHandlerV1 : Users.UsersBase
    {
        private readonly ILogger<UserHandlerV1> logger;
        private readonly ICreateUserUseCase createUserUseCase;

        public UserHandlerV1(ILogger<UserHandlerV1> logger, ICreateUserUseCase createUserUseCase)
        {
            this.logger = logger;
            this.createUserUseCase = createUserUseCase;
        }

        public override async Task<CreateUserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
        {
            var createUserInput = new CreateUserInput(request.Email, request.Password, request.Name, request.PhoneNumber);

            Domain.User createdUser = await this.createUserUseCase.ExecuteAsync(createUserInput);

            var response = new CreateUserResponse
            {
                Uuid = createdUser.Id.ToString(),
                Name = createdUser.Name,
                Email = createdUser.Email,
                PhoneNumber = createdUser.PhoneNumber,
            };

            return response;
        }
    }
}
