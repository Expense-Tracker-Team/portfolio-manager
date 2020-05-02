namespace Domain
{
    using System;

    public class User
    {
        public User(Guid id, string email, string passwordHash, string name, string phoneNumber)
        {
            if (id == Guid.Empty)
            {
                id = Guid.NewGuid();
            }

            this.Id = id;

            this.Email = email;
            this.PasswordHash = passwordHash;

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.Name = name;
            this.PhoneNumber = phoneNumber;
        }

        public Guid Id { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }
    }
}
