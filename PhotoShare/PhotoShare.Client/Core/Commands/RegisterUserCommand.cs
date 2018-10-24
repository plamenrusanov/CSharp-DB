﻿namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Contracts;
    using PhotoShare.Client.Core.Dtos;
    using PhotoShare.Services.Contracts;

    public class RegisterUserCommand : ICommand
    {
        private readonly IUserService service;

        public RegisterUserCommand(IUserService service)
        {
            this.service = service;
        }

        // RegisterUser <username> <password> <repeat-password> <email>
        public string Execute(string[] data)
        {
            string username = data[0];
            string password = data[1];
            string repeatPassword = data[2];
            string email = data[3];

            var registerUserDto = new RegisterUserDto
            {
                Username = username,
                Password = password,
                Email = email
            };

            if (!IsValid(registerUserDto))
            {
                throw new ArgumentException("Invalid data!");
            }

            var userExist = this.service.Exists(username);
            if (userExist)
            {
                throw new InvalidOperationException($"Username {username} is already taken!");
            }

            if (password != repeatPassword)
            {
                throw new ArgumentException("Passwords do not match!");
            }
            this.service.Register(username, password, email);

            return $"User {username} was registered successfully!";
        }

        private bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }
    }
}
