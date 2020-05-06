namespace UnitTests.Common
{
    using global::Persistence.Models;
    using System;

    public class UserDataModelBuilder
    {
        private Guid id;
        private string email;
        private string passwordHash;
        private string name;
        private string phoneNumber;

        public UserDataModelBuilder()
        {
            id = Guid.NewGuid();
            email = "test@email.com";
            passwordHash = "secret_password";
            name = "John Doe";
            phoneNumber = "08888888888";
        }

        public UserDataModelBuilder WithId(Guid id)
        {
            this.id = id;
            return this;
        }

        public UserDataModelBuilder WithEmail(string email)
        {
            this.email = email;
            return this;
        }

        public User Build()
        {
            return new User
            {
                Id = id,
                Name = name,
                Email = email,
                PasswordHash = passwordHash,
                PhoneNumber = phoneNumber
            };
        }
    }
}
