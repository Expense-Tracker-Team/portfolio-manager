namespace UsersClient.Worker
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Grpc.Net.Client;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Google.Protobuf.WellKnownTypes;
    using Grpc.Core;
    using Api.Protos;

    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;

        public Worker(ILogger<Worker> logger) => this.logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    this.logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                    AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
                    AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

                    var channel = GrpcChannel.ForAddress("http://localhost:5001/");
                    var client = new Users.UsersClient(channel);


                    // GetUsers
                    GetUsersResponse users = await client.GetUsersAsync(new Empty());
                    var usersCount = users.Users.Count;
                    // this.logger.LogInformation($"Users count {usersCount}");


                    // GetUserAsyncStream
                    AsyncServerStreamingCall<GetUserStreamResponse> usersStream = client.GetUserAsyncStream(new Empty());
                    var usersStreamCount = 0;
                    await foreach (GetUserStreamResponse response in usersStream.ResponseStream.ReadAllAsync())
                    {
                        // response.Message
                        usersStreamCount++;
                        // this.logger.LogInformation($"Users stream count {usersStreamCount}");
                    }


                    // GetUserById
                    try
                    {
                        await client.GetUserByIdAsync(new GetUserByIdRequest
                        {
                            Uuid = Guid.NewGuid().ToString()
                        });
                    }
                    catch (RpcException ex)
                    {
                        this.logger.LogWarning(ex.Status.StatusCode.ToString());
                    }


                    // CreateUser
                    await client.CreateUserAsync(new CreateUserRequest
                    {
                        Email = "test",
                        Name = "test",
                        PhoneNumber = "test"
                    });


                    CreateUserResponse newUser = await client.CreateUserAsync(new CreateUserRequest
                    {
                        Email = "test",
                        Name = "test",
                        PhoneNumber = "test"
                    });


                    // DeleteUser
                    await client.DeleteUserAsync(new DeleteUserRequest
                    {
                        Uuid = newUser.User.Uuid
                    });


                    Random rand = new Random();
                    var delay = rand.Next(1, 5000);
                    await Task.Delay(delay, stoppingToken);
                }
                catch (Exception exception)
                {
                    // ignored
                }
            }
        }
    }
}
