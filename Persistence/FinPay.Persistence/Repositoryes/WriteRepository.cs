using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinPay.Persistence.Context;
using FinPay.Application.Repositoryes;
using System.Runtime.CompilerServices;
using FinPay.Domain.Entity;

namespace FinPay.Persistence.Repositoryes
{
    public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;

        public WriteRepository(AppDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public async Task<bool> Add(T t)
        {
           EntityEntry entity = await Table.AddAsync(t);
            return entity.State == EntityState.Added;
        }

        public async Task<bool> AddRange(List<T> t)
        {
          await Table.AddRangeAsync(t);
            return true;
        }

        public bool Delete(int id)
        {
           var data = Table.FirstOrDefault(x => x.Id == id);
          EntityEntry entityEntry = Table.Remove(data);
            return EntityState.Deleted == entityEntry.State;    
        }

        public bool DeleteRange(T datas)
        { 
            Table.RemoveRange(datas);
            return true;  
        }

        public async Task<int> SaveAsync()
        {
           return await _context.SaveChangesAsync();
        }

        public bool Update(T t)
        {
          EntityEntry entityEntry = Table.Update(t);
            return entityEntry.State == EntityState.Modified;
        }
    }
}
