using FinPay.Application.DTOs;
using FinPay.Domain.Entity.Paymet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Service.Payment
{
    public interface IUserAccountService
    {
        public Task<bool> CreateUserAccount(UserAccountDto account);
        public Task<GetUserBalanceResponse> GetCardBalanceByAccountId(int userAccountId);
    }
}
