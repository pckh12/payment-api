using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payment.API.Domain.Model;
using System;

namespace Payment.API.Infrastructure.EntityConfigurations
{
    class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .IsRequired()
                .ValueGeneratedNever();

            builder.Property(a => a.Balance)
                .IsRequired();

            var navigation = builder.Metadata.FindNavigation(nameof(Account.PaymentRequests));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            // seed inital data - test account with initial balance
            builder.HasData(new Account(new Guid("6339d07a-430e-4029-a35c-13e815bcfab4"), 100));
        }
    }
}
