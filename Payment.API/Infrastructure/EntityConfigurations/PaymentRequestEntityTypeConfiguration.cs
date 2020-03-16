using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payment.API.Domain.Model;

namespace Payment.API.Infrastructure.EntityConfigurations
{
    class PaymentRequestEntityTypeConfiguration : IEntityTypeConfiguration<PaymentRequest>
    {
        public void Configure(EntityTypeBuilder<PaymentRequest> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .IsRequired()
                .ValueGeneratedNever();

            builder.Property(p => p.Date)
                .IsRequired();

            builder.Property(p => p.Amount)
                .IsRequired();

            builder.Property(p => p.Status)
                .IsRequired();

            builder.Property(p => p.Reason);

            builder.Property<int>("AccountId")
                .IsRequired();
        }
    }
}
