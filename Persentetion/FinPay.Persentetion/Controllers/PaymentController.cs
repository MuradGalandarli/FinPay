using FinPay.Application.Features.Commands.Payment.CaptureOrder;
using FinPay.Application.Features.Commands.Payment.PaymentTransaction;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace FinPay.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> PaymentTransaction(PaymentTransactionCommandRequest paymentTransactionCommandRequest)
        {
            PaymentTransactionCommandResponse paymentTransactionCommandResponse = await _mediator.Send(paymentTransactionCommandRequest);
            return Ok(paymentTransactionCommandResponse);

        }
        [HttpPost("CaptureOrder")]
        public async Task<IActionResult> CaptureOrderAsync([FromBody]CaptureOrderCommandRequest captureOrderCommandRequest)
        {
           CaptureOrderCommandResponse captureOrderCommandResponse = await _mediator.Send(captureOrderCommandRequest);
            return Ok(captureOrderCommandResponse);
        }
        string clientId = "ARJGMFf_CGpJPb7az_SDzIHpGg229LnTLKxIIYvPyPBwXLuh9jJW1ZYjrDqFwXfKibaJSXR1Qb3TkNgK";
        string clientSecret = "ELnTlKYVzNiGi76YU-Uhdfihbuq9IW9cj9igL8Y0UbvnVF8mzct5XNnWk1JTDLUCwVqoZhW-2znSTluY";

        
        private readonly string redirectUri = "https://localhost:7090/api/paypal/callback";

        //[HttpGet("login")]
        //public IActionResult LoginWithPayPal()
        //{
        //    string loginUrl =
        //        "https://www.sandbox.paypal.com/signin/authorize" +
        //        "?client_id=" + clientId +
        //        "&response_type=code" +
        //        "&redirect_uri=" + Uri.EscapeDataString(redirectUri) +
        //        "&scope=openid profile email";

        //    return Redirect(loginUrl);
        //}
        //[HttpGet("callback")]
        //public async Task<IActionResult> PayPalCallback([FromQuery] string code)
        //{
        //    if (string.IsNullOrEmpty(code))
        //        return BadRequest("Authorization code is missing");

        //    using var client = new HttpClient();
        //    var byteArray = Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}");
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

        //    var content = new FormUrlEncodedContent(new[]
        //    {
        //        new KeyValuePair<string, string>("grant_type", "authorization_code"),
        //        new KeyValuePair<string, string>("code", code),
        //        new KeyValuePair<string, string>("redirect_uri", redirectUri)
        //    });

        //    var response = await client.PostAsync("https://api-m.sandbox.paypal.com/v1/oauth2/token", content);
        //    var json = await response.Content.ReadAsStringAsync();

        //    if (!response.IsSuccessStatusCode)
        //        return StatusCode((int)response.StatusCode, json);

        //    var tokenObj = JsonConvert.DeserializeObject<PayPalTokenResponse>(json);

        //    // TODO: Save tokenObj to your database associated with current user

        //    return Ok(tokenObj);
        //}

        //public class PayPalTokenResponse
        //{
        //    public string access_token { get; set; }
        //    public string refresh_token { get; set; }
        //    public string token_type { get; set; }
        //    public int expires_in { get; set; }
        //    public string scope { get; set; }
        //}   


    }
}
