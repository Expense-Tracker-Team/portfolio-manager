namespace Application.UseCases.Models
{
    public class CreateUserInput
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }
    }
}
