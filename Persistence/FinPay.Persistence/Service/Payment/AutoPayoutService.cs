using FinPay.Application.Repositoryes;
using FinPay.Application.Repositoryes.AppTransactions;
using FinPay.Application.Repositoryes.CardBalance;
using FinPay.Application.Service;
using FinPay.Domain.Entity.Paymet;
using FinPay.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace FinPay.Persistence.Service.Payment
{
    public class AutoPayoutService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        

        public AutoPayoutService(IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();

                var transactionRepo = scope.ServiceProvider.GetRequiredService<ITransactionReadRepository>();
                var writeRepo = scope.ServiceProvider.GetRequiredService<ITransactionWriteRepository>();
                var cardBalanceReadRepo = scope.ServiceProvider.GetRequiredService<ICardBalanceReadRepository>();
                var cardBalanceWriteRepo = scope.ServiceProvider.GetRequiredService<ICardBalanceWriteRepository>();
                var userAccountWriteRepo = scope.ServiceProvider.GetRequiredService<IUserAccountWriteRepository>();
               
                var transactions = await transactionRepo
                    .GetWhere(x => x.Status == TransferStatus.Completed && !x.IsPayoutSent)
                    .ToListAsync();

                foreach (var tx in transactions)
                {
                    if (string.IsNullOrEmpty(tx.PaypalEmail)) continue;

                    var payoutSuccess = await SendPaypalPayout(tx.PaypalEmail, tx.Amount);

                    if (true)//payoutSuccess
                    {
                        tx.IsPayoutSent = true;
                        tx.Status = TransferStatus.Success;

                        var balance = await userAccountWriteRepo.Table.Include(x => x.CardBalance).Where(x=>x.CardBalance.IsActive == true).FirstOrDefaultAsync(x=>x.Id == tx.UserAccountId);
                        if (balance?.CardBalance == null)
                        {
                           
                            await cardBalanceWriteRepo.Add(new()
                            {
                                Balance = tx.Amount,
                                PaypalEmail = tx.PaypalEmail,
                                UserAccountId = tx.UserAccountId
                            });
                        }
                        else
                        {
                            balance.CardBalance.Balance += tx.Amount;
                        }
                    }

                    await writeRepo.SaveAsync();
                    await Task.Delay(500, stoppingToken);
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task<bool> SendPaypalPayout(string email, decimal amount)
        {
            using var httpClient = new HttpClient();

            var clientId = _configuration["PaypalSettings:clientId"];
            var clientSecret = _configuration["PaypalSettings:clientSecret"];

            var byteArray = Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var tokenContent = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");
            var tokenResponse = await httpClient.PostAsync("https://api-m.sandbox.paypal.com/v1/oauth2/token", tokenContent);
            var tokenResult = await tokenResponse.Content.ReadAsStringAsync();

            if (!tokenResponse.IsSuccessStatusCode)
                return false;

            dynamic tokenData = JsonConvert.DeserializeObject(tokenResult);
            string accessToken = tokenData.access_token;

            using var payoutClient = new HttpClient();
            payoutClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var payoutRequest = new
            {
                sender_batch_header = new
                {
                    sender_batch_id = Guid.NewGuid().ToString(),
                    email_subject = "You've received a payout!",
                    email_message = "You have received a payout from FinPay."
                },
                items = new[]
                {
                    new
                    {
                        recipient_type = "EMAIL",
                        amount = new
                        {
                            value = amount.ToString("F2", System.Globalization.CultureInfo.InvariantCulture),
                            currency = "USD"
                        },
                        receiver = email,
                        note = "Thanks for using FinPay!",
                        sender_item_id = Guid.NewGuid().ToString()
                    }
                }
            };

            var json = JsonConvert.SerializeObject(payoutRequest);
            var payoutContent = new StringContent(json, Encoding.UTF8, "application/json");

            var payoutResponse = await payoutClient.PostAsync("https://api-m.sandbox.paypal.com/v1/payments/payouts", payoutContent);
            var payoutResult = await payoutResponse.Content.ReadAsStringAsync();
            
            return payoutResponse.IsSuccessStatusCode;
        }
    }
}
