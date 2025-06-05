using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Repositoryes
{
    public interface IReadRepository<T> : IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetWhere(Expression<Func<T, bool>> methot);
        Task<T> GetSingelAsync(Expression<Func<T, bool>> methot);
        Task<T> GetByIdAsync(int id);
    }

}

