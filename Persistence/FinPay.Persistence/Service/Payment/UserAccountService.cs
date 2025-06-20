using FinPay.Application.DTOs;
using FinPay.Application.Enums;
using FinPay.Application.Repositoryes;
using FinPay.Application.Repositoryes.CardBalance;
using FinPay.Application.Repositoryes.UserAccount;
using FinPay.Application.Service.Payment;
using FinPay.Domain.Entity.Paymet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Persistence.Service.Payment
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IUserAccountReadRepository _userAccountReadRepository;
        private readonly IUserAccountWriteRepository _userAccountWriteRepository;
        private readonly ICardBalanceReadRepository _cardBalanceReadRepository;

        public UserAccountService(IUserAccountWriteRepository userAccountWriteRepository, IUserAccountReadRepository userAccountReadRepository, ICardBalanceReadRepository cardBalanceReadRepository)
        {
            _userAccountWriteRepository = userAccountWriteRepository;
            _userAccountReadRepository = userAccountReadRepository;
            _cardBalanceReadRepository = cardBalanceReadRepository;
        }



        public async Task<bool> CreateUserAccount(UserAccountDto account)
        {
            var userAccount = await _userAccountReadRepository.GetSingelAsync(x => x.UserId == account.UserId && x.Status == UserAccountStatus.Active);
            if (userAccount == null)
            {
                await _userAccountWriteRepository.Add(new()
                {
                    CreatedAt = DateTime.UtcNow,
                    Status = UserAccountStatus.Active,
                    LinkedPaypalEmail = account.LinkedPaypalEmail,
                    UserId = account.UserId,
                    Currency = account.Currency,

                });
                await _userAccountWriteRepository.SaveAsync();
                return true;
            } 
            return false;
        }

        public async Task<GetUserBalanceResponse> GetCardBalanceByAccountId(int userAccountId)
        {
         var userBalance =  await _cardBalanceReadRepository.GetByIdAsync(userAccountId);
            return new()
            { 
                PaypalEmail = userBalance.PaypalEmail,
                Balance = userBalance.Balance,
                UserAccountId = userBalance.UserAccountId,
             
            };

        }
    }
}
