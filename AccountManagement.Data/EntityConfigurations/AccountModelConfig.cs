using AccountManagement.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountManagement.Data.EntityConfigurations
{
    public class AccountModelConfig : IEntityTypeConfiguration<AccountModel>
    {
        public void Configure(EntityTypeBuilder<AccountModel> builder)
        {
            builder.ToTable("Accounts");
            builder.HasKey(model => model.Id);

            builder.Property(model => model.Email).IsRequired();

            builder.HasIndex(model => model.Email)
                   .IsUnique();
        }
    }
}