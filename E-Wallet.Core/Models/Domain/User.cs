﻿using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace EWallet.Core.Models.Domain
{
    public class User:IdentityUser, IEntity<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Wallet Wallet { get; set; }


        public User()
        {

        }

        public User(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserName = email;
        }
    }

}
