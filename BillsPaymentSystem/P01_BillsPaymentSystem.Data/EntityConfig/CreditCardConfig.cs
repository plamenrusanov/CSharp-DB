using Microsoft.EntityFrameworkCore;
using P01_BillsPaymentSystem.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace P01_BillsPaymentSystem.Data.EntityConfig
{
    public class CreditCardConfig : IEntityTypeConfiguration<CreditCard>
    {
        public void Configure(EntityTypeBuilder<CreditCard> builder)
        {
            builder
                .HasOne(x => x.PaymentMethod)
                .WithOne(c => c.CreditCard)
                .HasForeignKey<PaymentMethod>(fk => fk.CreditCardId);
        }
    }
}
