using Microsoft.EntityFrameworkCore;
using P01_BillsPaymentSystem.Data;
using P01_BillsPaymentSystem.Data.Models;
using System;
using System.Linq;

namespace P01_BillsPaymentSystem
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var context = new BillsPaymentSystemContext())
            {
                //Initializer.Seed(context);

                //UserDetails(context);

                PayBills(context);
            }
        }

        private static void PayBills(BillsPaymentSystemContext context)
        {
            User user = GetUser(context);
            decimal amount = decimal.Parse(Console.ReadLine());
            if (user != null)
            {
                var totalBankAccount = user.PaymentMethods.Where(x => x.BankAccount != null).Sum(x => x.BankAccount.Balance);
                var totalcreditCard = user.PaymentMethods.Where(x => x.CreditCard != null).Sum(x => x.CreditCard.LimitLeft);
                var totalAmount = totalBankAccount + totalcreditCard;

                if (totalAmount < amount)
                {
                    Console.WriteLine($"Insufficient funds!");
                }
                else
                {
                    var bankAccounts = user.PaymentMethods
                        .Where(x => x.BankAccount != null)
                        .Select(x => x.BankAccount)
                        .OrderBy(x => x.BankAccountId);

                    foreach (var ba in bankAccounts)
                    {
                        if (ba.Balance >= amount)
                        {
                            ba.Withdraw(amount);
                            amount = 0;
                        }
                        else
                        {
                            amount -= ba.Balance;
                            ba.Withdraw(ba.Balance);
                        }

                        if (amount == 0)
                        {
                            return;
                        }
                    }

                    var creditCards = user.PaymentMethods
                       .Where(x => x.CreditCard != null)
                       .Select(x => x.CreditCard)
                       .OrderBy(x => x.CreditCardId);

                    foreach (var cc in creditCards)
                    {
                        if (cc.LimitLeft >= amount)
                        {
                            cc.Withdraw(amount);
                            amount = 0;
                        }
                        else
                        {
                            amount -= cc.LimitLeft;
                            cc.Withdraw(cc.LimitLeft);
                        }

                        if (amount == 0)
                        {
                            return;
                        }
                    }
                }

            }

        }

        private static void UserDetails(BillsPaymentSystemContext context)
        {
            User user = GetUser(context);

            if (user != null)
            {
                PrintUserDetails(user);
            }
        }

        private static void PrintUserDetails(User user)
        {
            Console.WriteLine($"User: {user.FirstName} {user.LastName}");
            Console.WriteLine("Bank Accounts:");

            var bankAccounts = user
                .PaymentMethods
                .Where(x => x.BankAccount != null)
                .Select(x => x.BankAccount)
                .ToArray();

            foreach (var bankAccount in bankAccounts)
            {
                Console.WriteLine($"-- ID: {bankAccount.BankAccountId}");
                Console.WriteLine($"--- Balance: {bankAccount.Balance:f2}");
                Console.WriteLine($"--- Bank: {bankAccount.BankName}");
                Console.WriteLine($"--- SWIFT: {bankAccount.SWIFTCode}");
            }

            Console.WriteLine("Credit Cards:");

            var creditCards = user
              .PaymentMethods
              .Where(x => x.CreditCard != null)
              .Select(x => x.CreditCard)
              .ToArray();

            foreach (var CC in creditCards)
            {
                Console.WriteLine($"-- ID: 1");
                Console.WriteLine($"--- Limit: {CC.Limit:f2}");
                Console.WriteLine($"--- Money Owed: {CC.MoneyOwed:f2}");
                Console.WriteLine($"--- Limit Left:: {CC.LimitLeft:f2}");
                Console.WriteLine($"--- Expiration Date: {CC.ExpirationDate.Year}/{CC.ExpirationDate.Month}");
            }
        }

        private static User GetUser(BillsPaymentSystemContext context)
        {
            int userId = int.Parse(Console.ReadLine());

            User user = context
                .Users
                .Where(u => u.UserId == userId)
                .Include(x => x.PaymentMethods)
                .ThenInclude(x => x.BankAccount)
                .Include(x => x.PaymentMethods)
                .ThenInclude(x => x.CreditCard)
                .FirstOrDefault();

            if (user == null)
            {
                Console.WriteLine($"User with id {userId} not found!");
            }

            return user;
        }
    }
}
