using FinPay.Application.DTOs;
using FinPay.Application.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Features.Queries.AppUser.GetUser
{
    public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQueryRequest, GetAllUserQueryResponse>
    {
        private readonly IUserService _userService;
        private readonly IMetricsService _metricsService;



        public GetAllUserQueryHandler(IUserService userService, IMetricsService metricsService = null)
        {
            _userService = userService;
            _metricsService = metricsService;
        }

        public async Task<GetAllUserQueryResponse> Handle(GetAllUserQueryRequest request, CancellationToken cancellationToken)
        {
            _metricsService.IncrementCounter("GetAllUserQueryRequest");
            var sw = Stopwatch.StartNew();

            List<UserResponseDto> user = await _userService.GetAllUserAsync(request.Page, request.Size);
            sw.Stop();
            _metricsService.ObserveHistogram("GetAllUserQueryRequest_duration_seconds", sw.Elapsed.TotalSeconds);
            return new() 
            { 
                 Users = user 
            };
           


        }
    }
}
