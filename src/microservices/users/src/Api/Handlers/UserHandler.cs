namespace Api
{
    using System.Threading.Tasks;
    using Application.Interfaces;
    using Grpc.Core;
    using GrpcUsers;
    using Microsoft.Extensions.Logging;

    public class UserHandler : Users.UsersBase
    {
        private readonly ILogger<UserHandler> _logger;
        private readonly IUserService _userService;
        public UserHandler(ILogger<UserHandler> logger, IUserService userService)
        {
            this._logger = logger;
            this._userService = userService;
        }

        public override Task<User> CreateUser(CreateUserRequest request, ServerCallContext context)
        {
            return Task.FromResult(new User
            {
                Uuid =  System.Guid.NewGuid().ToString(),
                Email = request.Email,
                Name = request.Name
            });
        }

        public override Task<User> UpdateUser(UpdateUserRequest request, ServerCallContext context)
        {
            return Task.FromResult(new User
            {
                Uuid =  System.Guid.NewGuid().ToString(),
                Email = request.Email,
                Name = request.Name
            });
        }

        public override Task<User> GetUser(GetUserRequest request, ServerCallContext context)
        {
            
            return Task.FromResult(new User
            {
                Uuid =  System.Guid.NewGuid().ToString(),
                Email = "Protos for the win!",
                Name =  this._userService.GetUser()
            });
        }
    }
}
