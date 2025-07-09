
using FinPay.Domain.Identity;
using FinPay.Persistence.Context;
using FinPay.Persistence.Seeder;
using FinPay.Presentation.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FinPay.Application;
using FinPay._Infrastructure;
using FinPay.MessageRetryEngine;
using FinPay.Persistence;
using FinPay.SignalR.Hubs;
using FinPay.SignalR;
using FinPay.AutoMapper;
using Serilog;
using FinPay.Validator;
using FinPay.Presentation;
using Prometheus;


namespace FinPay.Persentetion
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<RolePermissionFilter>();
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddInfrastructureService();
            builder.Services.AddRabbitMQService();
            builder.Services.AddApplicationService();
            builder.Services.AddPersistenceRegistration(builder.Configuration);
            builder.Services.AddSignalRService(); 
            builder.Services.AddAutoMapperService();
            builder.Services.AddValidationService();
            builder.Services.AddApiLimiter(builder.Configuration);


            builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                            .AddEntityFrameworkStores<AppDbContext>()
                            .AddDefaultTokenProviders().AddDefaultTokenProviders();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }
 )
   .AddJwtBearer(options =>
   {
       options.SaveToken = true;
       options.RequireHttpsMetadata = false;
       options.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuer = true,
           ValidateAudience = true,
           ValidAudience = builder.Configuration["JWT:ValidAudience"],
           ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
           ClockSkew = TimeSpan.Zero,
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:secret"]))
       };
   }
    );
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseRateLimiter();

            app.UseRouting();

            app.UseRouting();

            app.UseHttpMetrics();            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapMetrics();
            });
            await DbSeeder.SeedData(app);
            app.Run();
        }
    }
}
