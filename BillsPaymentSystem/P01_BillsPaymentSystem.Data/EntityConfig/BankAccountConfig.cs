using Microsoft.EntityFrameworkCore;
using P01_BillsPaymentSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace P01_BillsPaymentSystem.Data.EntityConfig
{
    public class BankAccountConfig : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder
                .HasOne(x => x.PaymentMethod)
                .WithOne(x => x.BankAccount)
                .HasForeignKey<PaymentMethod>(x => x.BankAccountId);
        }
    }
}
