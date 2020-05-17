namespace UnitTests.Persistence.Helpers
{
    using global::Persistence.Models;
    using System;
    using System.Linq;

    public class FakeUserSet : FakeDbSet<User>
    {
        public override User Find(params object[] keyValues)
            => this.SingleOrDefault(e => e.Id == (Guid)keyValues.Single());
    }
}
