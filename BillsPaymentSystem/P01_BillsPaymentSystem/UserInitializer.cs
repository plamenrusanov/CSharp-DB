using P01_BillsPaymentSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_BillsPaymentSystem
{
     public class UserInitializer
    {
        public static User[] GetUsers()
        {
            User[] Users = new User[]
            {
                new User() {FirstName = "Gosho", LastName = "Petrov", Email = "petrov@abv.bg", Password =  "ouy45667"},
                new User() {FirstName = "Ivan", LastName = "Qnkov", Email = "ivan12@abv.bg", Password =  "qwe34567"},
                new User() {FirstName = "Milen", LastName = "Malinov", Email = "dahkd@dsj.ds", Password = "giuew4hy8" },
                new User() {FirstName = "Galq", LastName = "Marinova", Email = "gdewe@siei.er", Password =  "123432ed"},
                new User() {FirstName = "Katerina", LastName = "Svircheva", Email = "asg@okf.fd", Password = "98753hk3" },
                new User() {FirstName = "Petar", LastName = "Golemanov", Email = "golemanov@ds.f", Password =  "iru7hg4f"},
                new User() {FirstName = "Plamen", LastName = "Rusanov", Email = "dsj@jor.r" , Password = "uhurhuf84" },

            };
            return Users;
        }
    }
}
