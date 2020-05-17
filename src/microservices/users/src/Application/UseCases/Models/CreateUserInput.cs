namespace Application.UseCases.Models
{
    public class CreateUserInput
    {
        public CreateUserInput(string email, string password, string name, string phoneNumber)
        {
            this.Email = email;
            this.Password = password;
            this.Name = name;
            this.PhoneNumber = phoneNumber;
        }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }
    }
}
