namespace PhotoShare.Client.Core.Commands
{
    using System;
    using Contracts;
    using PhotoShare.Client.Core.Dtos;
    using PhotoShare.Client.Core.Validation;
    using PhotoShare.Services;
    using PhotoShare.Services.Contracts;

    public class ModifyUserCommand : ICommand
    {

        private readonly IUserService userService;
        private readonly ITownService townService;
        private PasswordAttribute passwordAttribute;

        public ModifyUserCommand(ITownService townService, IUserService userService)
        {
            this.userService = userService;
            this.townService = townService;
            this.passwordAttribute = new PasswordAttribute(4, 12);
        }

       
        // ModifyUser <username> <property> <new value>
        // For example:
        // ModifyUser <username> Password <NewPassword>
        // ModifyUser <username> BornTown <newBornTownName>
        // ModifyUser <username> CurrentTown <newCurrentTownName>
        // !!! Cannot change username
        public string Execute(string[] data)
        {
            string username = data[0];
            string property = data[1];
            string newValue = data[2];

            var userExist = this.userService.Exists(username);

            if (!userExist)
            {
                throw new ArgumentException($"User {username} not found!");
            }

            var userId = this.userService.ByUsername<UserDto>(username).Id;

            if (property == "Password")
            {
                ChangePassword(newValue, userId);
            }
            else if (property == "BornTown")
            {
                SetBornTown(newValue, userId);
            }
            else if (property == "CurrentTown")
            {
                SetCurrentTown(newValue, userId);
            }
            else
            {
                throw new ArgumentException($"Property {property} not supported!");
            }

            return $"User {username} {property} is {newValue}.";
        }

        private void SetCurrentTown(string newCurrentTown, int userId)
        {
            if (this.townService.Exists(newCurrentTown))
            {
                int townId = townService.ByName<TownDto>(newCurrentTown).Id;
                userService.SetCurrentTown(userId, townId);
            }
            else
            {
                throw new ArgumentException($"Value {newCurrentTown} not valid.\nTown {newCurrentTown} not found!”");
            }
        }

        private void SetBornTown(string newBornTown, int userId)
        {
            if (this.townService.Exists(newBornTown))
            {
                int townId = townService.ByName<TownDto>(newBornTown).Id;
                userService.SetBornTown(userId, townId);
            }
            else
            {
                throw new ArgumentException($"Value {newBornTown} not valid.\nTown {newBornTown} not found!”");
            }
        }

        private void ChangePassword(string newPassword, int userId)
        {
            if (passwordAttribute.IsValid(newPassword))
            {
                userService.ChangePassword(userId, newPassword);
            }
            else
            {
                throw new ArgumentException($"Value {newPassword} not valid.\nInvalid Password");
            }
        }
    }
}
