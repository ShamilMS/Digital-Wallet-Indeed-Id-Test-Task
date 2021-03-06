﻿using System.Collections.Generic;

namespace EWallet.Core.Models.Domain
{
    /// <summary>
    /// Digit wallet that belongs to one user and hold many Accounts
    /// </summary>
    public class Wallet : IEntity<string>
    {
        public string Id { get; set; }

        public Wallet()
        {
            Accounts = new HashSet<Account>();
        }

        #region Navigation Properties

        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<Account> Accounts { get; set; }
        #endregion

    }
}
