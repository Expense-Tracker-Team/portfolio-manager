using Application.Interfaces;

namespace Application{
    
    public class UserService : IUserService
    {
        public string GetUser() => "hello";
    }

}