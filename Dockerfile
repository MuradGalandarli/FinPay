FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5290


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Persentetion/FinPay.Persentetion/FinPay.Persentetion.csproj", "Persentetion/FinPay.Persentetion/"]
COPY ["Core/FinPay.Application/FinPay.Application.csproj", "Core/FinPay.Application/"]
COPY ["Core/FinPay.Domain/FinPay.Domain.csproj", "Core/FinPay.Domain/"]
COPY ["Core/FinPay.AutoMapper/FinPay.AutoMapper.csproj", "Core/FinPay.AutoMapper/"]
COPY ["Core/FinPay.Validator/FinPay.Validator.csproj", "Core/FinPay.Validator/"]
COPY ["FinPay.SignalR/FinPay.SignalR.csproj", "FinPay.SignalR/"]
COPY ["Infrasturcture/FinPay.Infrasturcture/FinPay.Infrastructure.csproj", "Infrasturcture/FinPay.Infrasturcture/"]
COPY ["MessageRetryEngine/FinPay.MessageRetryEngine.csproj", "MessageRetryEngine/"]
COPY ["Persistence/FinPay.Persistence/FinPay.Persistence.csproj", "Persistence/FinPay.Persistence/"]
RUN dotnet restore "./Persentetion/FinPay.Persentetion/FinPay.Persentetion.csproj"
COPY . .
WORKDIR "/src/Persentetion/FinPay.Persentetion"
RUN dotnet publish "./FinPay.Persentetion.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FinPay.Persentetion.dll"]
