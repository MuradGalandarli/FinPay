var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.FinPay_Persentetion>("finpay-persentetion");

builder.Build().Run();
