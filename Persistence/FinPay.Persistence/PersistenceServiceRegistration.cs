using FinPay.Application.Repositoryes.AppTransactions;
using FinPay.Application.Repositoryes.CardBalance;
using FinPay.Application.Repositoryes.Endpoint;
using FinPay.Application.Repositoryes.Menu;
using FinPay.Application.Repositoryes.PaypalTransaction;
using FinPay.Application.Repositoryes.UserAccount;
using FinPay.Application.Repositoryes;
using FinPay.Application.Service.Authentications;
using FinPay.Application.Service.Payment;
using FinPay.Application.Service;
using FinPay.Persistence.Repositoryes.AppTransactions;
using FinPay.Persistence.Repositoryes.Endpoint;
using FinPay.Persistence.Repositoryes.Menu;
using FinPay.Persistence.Repositoryes.PaypalTransaction;
using FinPay.Persistence.Repositoryes.UserAccount;
using FinPay.Persistence.Repositoryes;
using FinPay.Persistence.Service.Authentications;
using FinPay.Persistence.Service.Payment;
using FinPay.Persistence.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinPay.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FinPay.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static void AddPersistenceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("default");

            services.AddDbContext<AppDbContext>(options =>
                                      options.UseNpgsql(connectionString));

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddHostedService<AutoPayoutService>();
            services.AddScoped<IMenuWriteRepository, MenuWriteRepository>();
            services.AddScoped<IMenuReadRepository, MenuReadRepository>();
            services.AddScoped<IEndpointReadRepository, EndpointReadRepository>();
            services.AddScoped<IEndpointWriteRepository, EndpointWriteRepository>();
            services.AddScoped<IAuthorizationEndpointService, AuthorizetionEndpointService>();
            services.AddScoped<IPaymentTransaction, PaymentTransaction>();
            services.AddScoped<ITransactionReadRepository, TransactionReadRepository>();
            services.AddScoped<ITransactionWriteRepository, TransactionWriteRepository>();
            services.AddScoped<ICardBalanceReadRepository, CardBalanceReadRepository>();
            services.AddScoped<ICardBalanceWriteRepository, CardBalanceWriteRepository>();
            services.AddScoped<IPaypalTransactionWriteRepository, PeypalTransactionWriteRepository>();
            services.AddScoped<IPaypalTransactionReadRepository, PeypalTransactionReadRepository>();
            services.AddScoped<ICardTransactionService, CardTransactionService>();
            services.AddScoped<IUserAccountService, UserAccountService>();
            services.AddScoped<IUserAccountWriteRepository, UserAccountWriteRepository>();
            services.AddScoped<IUserAccountReadRepository, UserAccountReadRepository>();
            services.AddHostedService<SqlBackupService>();


        }
    }
}
