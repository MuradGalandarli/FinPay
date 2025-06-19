using FinPay.Application.RabbitMqMessage;
using FinPay.Application.Repositoryes.AppTransactions;
using FinPay.Application.Service;
using FinPay.Application.Service.Payment;
using FinPay.Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinPay.Persistence.Service.Payment
{
    public class PaymentTransaction : IPaymentTransaction
    {
        private readonly ITransactionWriteRepository _transactionWriteRepository;
        private readonly ITransactionReadRepository _transactionReadRepository;
        private readonly IConfiguration _configuration;
        private readonly IRabbitMqPublisher _rabbitMqPublisher;

        public PaymentTransaction(ITransactionWriteRepository transactionWriteRepository, ITransactionReadRepository transactionReadRepository, IConfiguration configuration, IRabbitMqPublisher rabbitMqPublisher)
        {
            _transactionWriteRepository = transactionWriteRepository;
            _transactionReadRepository = transactionReadRepository;
            _configuration = configuration;
            _rabbitMqPublisher = rabbitMqPublisher;
        }

        public async Task<string> CreatePayment(decimal amount,string userId)
        {
            string accessToken = await GetAccessTokenAsync();
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var requestBody = new
            {
                intent = "CAPTURE",
                purchase_units = new[]
                {
            new
            {
                amount = new
                {
                  currency_code = "USD",
                  value = amount

                }
            }
        },
                application_context = new
                {
                    return_url = "https://your-site.com/success",
                    cancel_url = "https://your-site.com/cancel"
                }
            };

           

            await _rabbitMqPublisher.Publish("transaction-exchange", "transactionKey", new CreatePaymentMQ
            {
                IsPayoutSent = true,
                Amount = amount,
                CreateAt = DateTime.UtcNow,
                FromUserId = userId,
                Status = TransferStatus.Created,
                PaypalEmail = " "
             
            });

            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await httpClient.PostAsync("https://api-m.sandbox.paypal.com/v2/checkout/orders", content);
            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;  
        }

        public async Task<string> CaptureOrderAsync(string orderId, string userId)
        {
            string accessToken = await GetAccessTokenAsync();

            using var httpClient = new HttpClient(); 
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var content = new StringContent("{}", Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(
                $"https://api-m.sandbox.paypal.com/v2/checkout/orders/{orderId}/capture",
                content);

            var result = await response.Content.ReadAsStringAsync();

            var transaction = await _transactionReadRepository
                .GetSingelAsync(x => x.FromUserId == userId && x.Status == TransferStatus.Created);

            if (transaction == null)
            {
                return "Transaction not found.";
            }

            string status = null;
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var json = JsonDocument.Parse(result);
                    status = json.RootElement.GetProperty("status").GetString();
                }
                catch (Exception ex)
                {
                    // Log ex
                    transaction.Status = TransferStatus.Failed;
                    await _transactionWriteRepository.SaveAsync();
                    return "Invalid response from PayPal.";
                }
            }

            if (response.IsSuccessStatusCode && status == "COMPLETED")
            {
                transaction.Status = TransferStatus.Completed;
            }
            else
            {
                transaction.Status = TransferStatus.Failed;
            }

            await _transactionWriteRepository.SaveAsync();
            return result;
        }


        public async Task<string> GetAccessTokenAsync()
        {
            using var httpClient = new HttpClient();
            var byteArray = Encoding.ASCII.GetBytes($"{_configuration["PaypalSettings:clientId"]}:{_configuration["PaypalSettings:clientSecret"]}");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await httpClient.PostAsync("https://api-m.sandbox.paypal.com/v1/oauth2/token", content);
            var responseString = await response.Content.ReadAsStringAsync();

            dynamic result = JsonConvert.DeserializeObject(responseString);
            return result.access_token;
        }

    }
}
