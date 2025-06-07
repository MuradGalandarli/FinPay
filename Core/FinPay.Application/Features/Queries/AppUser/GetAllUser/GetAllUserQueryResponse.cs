using FinPay.Application.DTOs;

namespace FinPay.Application.Features.Queries.AppUser.GetUser
{
    public class GetAllUserQueryResponse
    {
        //public string Id { get; set; }
        //public string Name { get; set; }
        //public string Emil { get; set; }
        public List<UserResponseDto> Users { get; set; }
    }
}