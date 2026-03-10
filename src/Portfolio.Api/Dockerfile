FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files for restore (including all ProjectReference dependencies)
COPY Portfolio.Shared/Portfolio.Shared.csproj Portfolio.Shared/
COPY Portfolio.Data.CosmosDb/Portfolio.Data.CosmosDb.csproj Portfolio.Data.CosmosDb/
COPY Portfolio.Data.MySql/Portfolio.Data.MySql.csproj Portfolio.Data.MySql/
COPY Portfolio.Data.PostgreSql/Portfolio.Data.PostgreSql.csproj Portfolio.Data.PostgreSql/
COPY Portfolio.Sms.Abstractions/Portfolio.Sms.Abstractions.csproj Portfolio.Sms.Abstractions/
COPY Portfolio.Sms.ClickSend/Portfolio.Sms.ClickSend.csproj Portfolio.Sms.ClickSend/
COPY Portfolio.Sms.Twilio/Portfolio.Sms.Twilio.csproj Portfolio.Sms.Twilio/
COPY Portfolio.Api/Portfolio.Api.csproj Portfolio.Api/

RUN dotnet restore Portfolio.Api/Portfolio.Api.csproj

# Copy all source files and publish
COPY Portfolio.Shared/ Portfolio.Shared/
COPY Portfolio.Data.CosmosDb/ Portfolio.Data.CosmosDb/
COPY Portfolio.Data.MySql/ Portfolio.Data.MySql/
COPY Portfolio.Data.PostgreSql/ Portfolio.Data.PostgreSql/
COPY Portfolio.Sms.Abstractions/ Portfolio.Sms.Abstractions/
COPY Portfolio.Sms.ClickSend/ Portfolio.Sms.ClickSend/
COPY Portfolio.Sms.Twilio/ Portfolio.Sms.Twilio/
COPY Portfolio.Api/ Portfolio.Api/

RUN dotnet publish Portfolio.Api/Portfolio.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Create a directory for SQLite database persistence
RUN mkdir -p /app/data

COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "Portfolio.Api.dll"]
