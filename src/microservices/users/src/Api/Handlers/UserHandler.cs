namespace Api
{
    using System.Threading.Tasks;

    using Application.Interfaces;

    using Grpc.Core;

    using GrpcUsers;

    using Microsoft.Extensions.Logging;

    public class UserHandler : Users.UsersBase
    {
        private readonly ILogger<UserHandler> logger;
        private readonly IUserService userService;

        public UserHandler(ILogger<UserHandler> logger, IUserService userService)
        {
            this.logger = logger;
            this.userService = userService;
        }

        public override Task<User> CreateUser(CreateUserRequest request, ServerCallContext context)
        {
            return Task.FromResult(new User
            {
                Uuid = System.Guid.NewGuid().ToString(),
                Email = request.Email,
                Name = request.Name
            });
        }

        public override Task<User> UpdateUser(UpdateUserRequest request, ServerCallContext context)
        {
            return Task.FromResult(new User
            {
                Uuid = System.Guid.NewGuid().ToString(),
                Email = request.Email,
                Name = request.Name
            });
        }

        public override Task<User> GetUser(GetUserRequest request, ServerCallContext context)
        {
            return Task.FromResult(new User
            {
                Uuid = System.Guid.NewGuid().ToString(),
                Email = "Protos for the win!",
                Name = this.userService.GetUser()
            });
        }
    }
}
