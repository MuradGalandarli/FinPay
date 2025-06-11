
using FinPay._Infrastructure.Service.Configurations;
using FinPay.Application.Features.Commands.AppUser.SignupUser;
using FinPay.Application.Repositoryes.Endpoint;
using FinPay.Application.Repositoryes.Menu;
using FinPay.Application.Repositoryes.AppTransactions;
using FinPay.Application.Service;
using FinPay.Application.Service.Authentications;
using FinPay.Application.Service.Configurations;
using FinPay.Application.Service.Payment;
using FinPay.Domain.Identity;
using FinPay.Persistence.Context;
using FinPay.Persistence.Repositoryes.Endpoint;
using FinPay.Persistence.Repositoryes.Menu;
using FinPay.Persistence.Repositoryes.AppTransactions;
using FinPay.Persistence.Seeder;
using FinPay.Persistence.Service;
using FinPay.Persistence.Service.Authentications;
using FinPay.Persistence.Service.Payment;
using FinPay.Presentation.Filters;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FinPay.Persentetion
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //builder.Services.AddControllers();
            //builder.Services.AddScoped<RolePermissionFilter>();

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<RolePermissionFilter>();
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddMediatR(typeof(SignupCommandRequest).Assembly);


            builder.Services.AddScoped<IApplicationService, ApplicationService>();
            builder.Services.AddScoped<IMenuWriteRepository, MenuWriteRepository>();
            builder.Services.AddScoped<IMenuReadRepository, MenuReadRepository>();
            builder.Services.AddScoped<IEndpointReadRepository, EndpointReadRepository>();
            builder.Services.AddScoped<IEndpointWriteRepository, EndpointWriteRepository>();
            builder.Services.AddScoped<IAuthorizationEndpointService, AuthorizetionEndpointService>();
            builder.Services.AddScoped<IPaymentTransaction, PaymentTransaction>();
            builder.Services.AddScoped<ITransactionReadRepository, TransactionReadRepository>();
            builder.Services.AddScoped<ITransactionWriteRepository, TransactionWriteRepository>();


            string connectionString = builder.Configuration.GetConnectionString("default");

            builder.Services.AddDbContext<AppDbContext>(options =>
                                      options.UseNpgsql(connectionString));

            // For Identity
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                            .AddEntityFrameworkStores<AppDbContext>()
                            .AddDefaultTokenProviders();

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


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();



            app.MapControllers();
            await DbSeeder.SeedData(app);
            app.Run();
        }
    }
}
