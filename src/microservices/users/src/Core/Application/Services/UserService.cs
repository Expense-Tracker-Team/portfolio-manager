using UsersService.Application.Interfaces;

namespace UsersService.Application{
    
    public class UserService : IUserService
    {
        public string GetUser()
        {
            return "hello";
        }
    }

}