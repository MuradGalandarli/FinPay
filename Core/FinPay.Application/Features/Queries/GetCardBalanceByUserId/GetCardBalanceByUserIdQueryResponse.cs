namespace FinPay.Application.Features.Queries.GetCardBalanceByUserId
{
    public class GetCardBalanceByUserIdQueryResponse
    {
        public decimal Balance { get; set; }
        public string PaypalEmail { get; set; }
     
    }
}