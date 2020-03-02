﻿using EWallet.Core.Models.Domain;
using EWallet.Core.Services.Application;
using EWallet.Core.Services.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EWallet.Application.Services
{
    public class AccountService : IAccountService
    {
        public IRepository<Account> Repository { get; set; }

        public AccountService(IRepository<Account> repository)
            => Repository = repository;


        public async Task<(Account account, string errorMessage)> CreateAccount(Action<IAccountBuilder> builderOptions)
        {
            var newAccount = new Account();
            builderOptions(new AccountBuilder(newAccount));

            if (!await Repository.Set().AnyAsync(AccountExist(newAccount)))
            {
                await Repository.Set().AddAsync(newAccount);
                await Repository.SaveChangesAsync();
                return (newAccount, errorMessage: string.Empty);
            }

            return (account: null, "Account already exists!");
        }


        public async Task IncreaseBalance(Account account, decimal amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException("Amount parameter is less thatn zero");

            account.Balance += amount;
            Repository.Set().Update(account);
            await Repository.SaveChangesAsync();
        }


        public async Task<(bool succeeded, string errorMessage)> DecreaseBalance(Account account, decimal amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException("Amount parameter is less than zero");

            if (account.Balance >= amount)
            {
                account.Balance -= amount;
                Repository.Set().Update(account);
                await Repository.SaveChangesAsync();

                return (true, errorMessage: string.Empty);
            }

            return (false, errorMessage: "Insufficient funds");
        }

        private Expression<Func<Account, bool>> AccountExist(Account account)
            => x => x.WalletId == account.WalletId
                 && x.Currency == account.Currency;
    }
}
