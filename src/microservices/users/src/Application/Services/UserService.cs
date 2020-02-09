namespace Application.Services
{
    using Application.Interfaces;

    public class UserService : IUserService
    {
        public string GetUser() => "hello";
    }
}