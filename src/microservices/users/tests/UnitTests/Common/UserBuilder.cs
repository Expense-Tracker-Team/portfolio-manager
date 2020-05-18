namespace UnitTests.Common
{
    using global::Domain;
    using System;

    public class UserBuilder
    {
        private Guid id;
        private string email;
        private string passwordHash;
        private string name;
        private string phoneNumber;

        public UserBuilder()
        {
            id = Guid.NewGuid();
            email = "test@email.com";
            passwordHash = "secret_password";
            name = "John Doe";
            phoneNumber = "08888888888";
        }

        public UserBuilder WithId(Guid id)
        {
            this.id = id;
            return this;
        }

        public UserBuilder WithEmail(string email)
        {
            this.email = email;
            return this;
        }

        public User Build() => new User(id, email, passwordHash, name, phoneNumber);
    }
}
