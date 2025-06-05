using FinPay.Application.Repositoryes;
using FinPay.Domain.Entity;
using FinPay.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Persistence.Repositoryes
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
         private readonly AppDbContext _appDbContext;

        public ReadRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public DbSet<T> Table => _appDbContext.Set<T>();

        public IQueryable<T> GetAll() => Table.AsQueryable();

        public async Task<T> GetByIdAsync(int id) => await Table.FirstOrDefaultAsync(x => x.Id == id);
       

        public async Task<T> GetSingelAsync(Expression<Func<T, bool>> methot)
        {
         return await Table.AsQueryable().FirstOrDefaultAsync(methot);
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> methot)
        {
           return Table.AsQueryable().Where(methot);    
        }
    }
}
