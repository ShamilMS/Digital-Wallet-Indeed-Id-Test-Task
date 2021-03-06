﻿using EWallet.Application.Builders;
using EWallet.Core.Models.Domain;
using EWallet.Core.Services.Application;
using EWallet.Core.Services.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EWallet.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<Operation> operationRepository;
        private readonly ICurrencyHelper currencyHelper;
        public IRepository<Account> Repository { get; set; }

        public AccountService(IRepository<Account> repository,
                              IRepository<Operation> operationRepository,
                             ICurrencyHelper currencyHelper)
        {
            Repository = repository;
            this.operationRepository = operationRepository;
            this.currencyHelper = currencyHelper;
        }

        public async Task<(Account account, string errorMessage)> CreateAccountAsync(Action<IAccountBuilder> builderOptions)
        {
            var builder = new AccountBuilder();
            builderOptions(builder);
            Account newAccount = builder.Build();

            if (!await Repository.Set().AnyAsync(AccountExist(newAccount)))
            {
                await Repository.Set().AddAsync(newAccount);
                await Repository.SaveChangesAsync();
                return (newAccount, errorMessage: string.Empty);
            }

            return (null, "Such account already exists!");
        }



        public async Task<(bool succeeded, string errorMessage)> DecreaseBalanceAsync(Account account, decimal amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException("Amount parameter is less than zero");

            if (account.Balance >= amount)
            {
                account.Balance -= amount;
                Repository.Set().Update(account);
                await Repository.SaveChangesAsync();

                operationRepository.Set().Add(new Operation(account.Id, amount, OperationDirection.Out));
                await operationRepository.SaveChangesAsync();

                return (true, errorMessage: string.Empty);
            }

            return (false, errorMessage: "Insufficient funds");
        }

        private Expression<Func<Account, bool>> AccountExist(Account account)
            => x => x.WalletId == account.WalletId
                 && x.CurrencyIsoNumberCode == account.CurrencyIsoNumberCode;

        public async Task<(bool succeeded, string errorMessage)> IncreaseBalanceAsync(Account account, decimal amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException("Amount parameter is less thatn zero");

            account.Balance += amount;
            Repository.Set().Update(account);
            await Repository.SaveChangesAsync();

            operationRepository.Set().Add(new Operation(account.Id, amount, OperationDirection.In));
            await operationRepository.SaveChangesAsync();

            return (true, string.Empty);
        }


        public async Task<(bool succeeded, string errorMessage)> TransferAmount(Account accountFrom, Account accountTo, decimal transferAmount)
        {
            if (transferAmount < 0)
                throw new ArgumentOutOfRangeException("Amount parameter is less thatn zero");

            if (accountFrom.Balance >= transferAmount)
            {
                decimal convertedAmount = await currencyHelper.ConvertAsync(accountFrom.Currency, accountTo.Currency, transferAmount);

                using (var transaction = await Repository.Context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await DecreaseBalanceAsync(accountFrom, transferAmount);
                        await IncreaseBalanceAsync(accountTo, convertedAmount);

                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
                
                return (true, errorMessage: string.Empty);
            }

            return (false, errorMessage: "Insufficient funds");
        }
    }
}

