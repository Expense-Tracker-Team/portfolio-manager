namespace Persistence.Models
{
    using System;

    public class User
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }
    }
}
