﻿using EWallet.Core.Services.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace EWallet.Persistence.Services
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationContext context;

        public Repository(ApplicationContext ctx)
            => context = ctx;


        public DbContext Context { get => context; }

        public DbSet<T> Set()
            => context.Set<T>();

        public event Func<IRepositoryBase<T>, Task> OnRepositoryUpdateAsync;

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();

            if(!(OnRepositoryUpdateAsync is null))
                await OnRepositoryUpdateAsync.Invoke(this);
        }
    }
}
