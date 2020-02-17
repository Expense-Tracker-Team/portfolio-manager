namespace Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Api.Protos;
    using Google.Protobuf.WellKnownTypes;
    using Grpc.Core;
    using Microsoft.Extensions.Logging;
    using Serilog;
    using Serilog.Context;
    
    public class UserHandlerV1 : Users.UsersBase
    {
        private ILogger<UserHandlerV1> logger;

        public UserHandlerV1(ILogger<UserHandlerV1> logger){
            this.logger = logger;
        }
        private static readonly List<User> usersList = new List<User>();

        public override Task<GetUsersResponse> GetUsers(Empty request, ServerCallContext context)
        {
            var response = new GetUsersResponse();
            response.Users.AddRange(usersList);

            return Task.FromResult(response);
        }

        public override Task GetUserAsyncStream(Empty request, IServerStreamWriter<GetUserStreamResponse> responseStream, ServerCallContext context)
        {
            foreach (User user in usersList)
            {
                responseStream.WriteAsync(new GetUserStreamResponse()
                {
                    User = user
                });
            }

            return Task.CompletedTask;
        }

        public override Task<GetUserByIdResponse> GetUserById(GetUserByIdRequest request, ServerCallContext context)
        {
            bool usersExist = usersList.Any(u => u.Uuid == request.Uuid);
            if (!usersExist)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"User with id {request.Uuid} was not found"));
            }

            var response = new GetUserByIdResponse
            {
                User = usersList.FirstOrDefault(u => u.Uuid == request.Uuid)
            };

            return Task.FromResult(response);
        }

        public override Task<CreateUserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
        {
            var user = new User
            {
                Uuid = Guid.NewGuid().ToString(),
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            usersList.Add(user);

            var response = new CreateUserResponse
            {
                User = user
            };

            return Task.FromResult(response);
        }

        public override Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request, ServerCallContext context) => Task.FromResult(new UpdateUserResponse());

        public override Task<DeleteUserResponse> DeleteUser(DeleteUserRequest request, ServerCallContext context)
        {
            bool usersExist = usersList.Any(u => u.Uuid == request.Uuid);
            if (!usersExist)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"User with id {request.Uuid} was not found"));
            }

            User user = usersList.FirstOrDefault(u => u.Uuid == request.Uuid);
            usersList.Remove(user);

            var response = new DeleteUserResponse
            {
                User = user
            };

            return Task.FromResult(response);
        }
    }
}
