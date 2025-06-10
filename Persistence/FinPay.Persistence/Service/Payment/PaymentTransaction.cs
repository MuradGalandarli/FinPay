using FinPay.Application.Service.Payment;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Persistence.Service.Payment
{
    public class PaymentTransaction : IPaymentTransaction
    {
        public async Task<string> CreatePayment(decimal amount)
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

            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://api-m.sandbox.paypal.com/v2/checkout/orders", content);
            var responseString = await response.Content.ReadAsStringAsync();

            return responseString; // içində approve URL və orderId olacaq
        }
            public async Task<string> CaptureOrderAsync(string orderId)
            {
            string accessToken = await GetAccessTokenAsync();

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Boş content JSON formatında
            var content = new StringContent("", Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(
                $"https://api-m.sandbox.paypal.com/v2/checkout/orders/{orderId}/capture",
                content); // null yerinə content

            var result = await response.Content.ReadAsStringAsync();
            return result; // status: COMPLETED gəlməlidir
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var clientId = "ARJGMFf_CGpJPb7az_SDzIHpGg229LnTLKxIIYvPyPBwXLuh9jJW1ZYjrDqFwXfKibaJSXR1Qb3TkNgK";
            var clientSecret = "ELnTlKYVzNiGi76YU-Uhdfihbuq9IW9cj9igL8Y0UbvnVF8mzct5XNnWk1JTDLUCwVqoZhW-2znSTluY";

            using var httpClient = new HttpClient();
            var byteArray = Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await httpClient.PostAsync("https://api-m.sandbox.paypal.com/v1/oauth2/token", content);
            var responseString = await response.Content.ReadAsStringAsync();

            dynamic result = JsonConvert.DeserializeObject(responseString);
            return result.access_token;
        }
       
    }
}
