namespace Domain
{
    using Domain.Exceptions;
    using System;
    using System.Text.RegularExpressions;

    public class User
    {
        private string email;
        private string name;

        public User(Guid id, string email, string passwordHash, string name, string phoneNumber)
        {
            if (id == Guid.Empty)
            {
                id = Guid.NewGuid();
            }

            this.Id = id;

            this.Email = email;
            this.PasswordHash = passwordHash;
            this.Name = name;
            this.PhoneNumber = phoneNumber;
        }

        public Guid Id { get; set; }

        public string Email
        {
            get => this.email;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new DomainException($"Email is required.");
                }

                // email validation regex
                var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(value);
                if (!match.Success)
                {
                    throw new DomainException($"Invalid email format.");
                }

                this.email = value;
            }
        }

        public string PasswordHash { get; set; }

        public string Name
        {
            get => this.name;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new DomainException($"Name is required.");
                }

                if (value.Length < 2)
                {
                    throw new DomainException($"Name length must be at least 2 characters");
                }

                this.name = value;
            }
        }

        public string PhoneNumber { get; set; }
    }
}
