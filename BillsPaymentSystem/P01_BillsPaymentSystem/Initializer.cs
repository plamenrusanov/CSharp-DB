using P01_BillsPaymentSystem.Data;
using System;
using System.Collections.Generic;
using System.Text;
using P01_BillsPaymentSystem.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace P01_BillsPaymentSystem
{
    public class Initializer
    {
        public static void Seed(BillsPaymentSystemContext context)
        {
            InsertUsers(context);
            //InsertCreditCards(context);
            //InsertBankAccount(context);
            InsertPaymentMethods(context);

        }

        private static void InsertPaymentMethods(BillsPaymentSystemContext context)
        {
            var paymentMethods = PaymentMethodsInitializer.GetPaymentMethods();

            for (int i = 0; i < paymentMethods.Length; i++)
            {
                if (IsValid(paymentMethods[i]))
                {
                    context.PaymentMethods.Add(paymentMethods[i]);
                }
            }
            context.SaveChanges();
        }

        //private static void InsertBankAccount(BillsPaymentSystemContext context)
        //{
        //    var bankAccount = BankAccountInitializer.GetBankAccounts();

        //    for (int i = 0; i < bankAccount.Length; i++)
        //    {
        //        if (IsValid(bankAccount[i]))
        //        {
        //            context.BankAccounts.Add(bankAccount[i]);
        //        }
        //    }
        //    context.SaveChanges();
        //}

        //private static void InsertCreditCards(BillsPaymentSystemContext context)
        //{
        //    var creditCards = CreditCardInitializer.GetCreditCards();

        //    for (int i = 0; i < creditCards.Length; i++)
        //    {
        //        if (IsValid(creditCards[i]))
        //        {
        //            context.CreditCards.Add(creditCards[i]);
        //        }
        //    }
        //    context.SaveChanges();
        //}

        private static void InsertUsers(BillsPaymentSystemContext context)
        {
            var users = UserInitializer.GetUsers();

            for (int i = 0; i < users.Length; i++)
            {
                if (IsValid(users[i])) 
                {
                    context.Users.Add(users[i]);
                }
            }
            context.SaveChanges();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var result = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, result, true);
        }
    }
}
