using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Repositoryes
{
    public interface IWriteRepository<T>:IRepository<T> where T : class
    {
       public Task<bool> Add(T t);
        public bool Update(T t);
        public bool Delete(int id);
        public bool DeleteRange(T datas);
        public Task<bool> AddRange(List<T> t);
        public Task<int> SaveAsync();


    }
}
