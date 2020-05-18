namespace Persistence.EntityTypeConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Persistence.Models;

    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<UserDataModel>
    {
        public void Configure(EntityTypeBuilder<UserDataModel> builder) => builder.HasKey(poll => poll.Id);
    }
}
