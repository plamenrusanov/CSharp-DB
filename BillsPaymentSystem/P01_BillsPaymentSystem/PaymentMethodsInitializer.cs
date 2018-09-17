using P01_BillsPaymentSystem.Data.Models;
using P01_BillsPaymentSystem.Data.Models.Attributes.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_BillsPaymentSystem
{
    public class PaymentMethodsInitializer
    {
        public static PaymentMethod[] GetPaymentMethods()
        {
            PaymentMethod[] paymentMethod = new PaymentMethod[]
            {
                new PaymentMethod(){UserId = 1, BankAccountId = 1, Type = PaymentType.BankAccount},
                new PaymentMethod(){UserId = 2, BankAccountId = 2, Type = PaymentType.CreditCard},
                new PaymentMethod(){UserId = 2, BankAccountId = 1, Type = PaymentType.BankAccount},
                new PaymentMethod(){UserId = 4, BankAccountId = 3, Type = PaymentType.BankAccount},
                new PaymentMethod(){UserId =1, BankAccountId = 3, Type = PaymentType.CreditCard},
                new PaymentMethod(){UserId = 1, BankAccountId = 5, Type = PaymentType.BankAccount},

            };
            return paymentMethod;
        }
    }
}
