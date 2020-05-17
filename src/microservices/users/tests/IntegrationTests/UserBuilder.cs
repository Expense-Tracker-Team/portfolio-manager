namespace IntegrationTests
{
    using Domain;
    using System;

    // Fluent builder test pattern
    public class UserBuilder
    {
        private Guid id;
        private string email;
        private string passwordHash;
        private string name;
        private string phoneNumber;

        public UserBuilder()
        {
            this.id = Guid.NewGuid();
            this.email = "test@email.com";
            this.passwordHash = "secret_password";
            this.name = "John Doe";
            this.phoneNumber = "08888888888";
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

        public User Build() => new User(this.id, this.email, this.passwordHash, this.name, this.phoneNumber);
    }
}
