using System.Threading.RateLimiting;

namespace FinPay.Presentetion
{
    public static class ApiRateLimiter
    {
        public static void AddApiLimiter(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddRateLimiter(options =>
            {
                string time = configuration.GetSection("RateLimiter:Time").Value;

                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                {
                    var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: ip,
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = int.Parse(configuration.GetSection("RateLimiter:RequestNumber").Value),
                            Window = TimeSpan.FromMinutes(int.Parse(time)),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0
                        });
                });

                options.OnRejected = async (ctx, ct) =>
                {
                    ctx.HttpContext.Response.Headers["Retry-After"] = time;
                    ctx.HttpContext.Response.ContentType = "application/json";
                    await ctx.HttpContext.Response.WriteAsync("""
        {
            "error": "Rate limit exceeded. Please wait and try again later."
        }
        """);
                };
            });


        }
    }
}
