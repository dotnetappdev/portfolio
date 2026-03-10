FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files for restore
COPY Portfolio.Shared/Portfolio.Shared.csproj Portfolio.Shared/
COPY Portfolio.Web/Portfolio.Web.csproj Portfolio.Web/

RUN dotnet restore Portfolio.Web/Portfolio.Web.csproj

# Copy all source files and publish
COPY Portfolio.Shared/ Portfolio.Shared/
COPY Portfolio.Web/ Portfolio.Web/

RUN dotnet publish Portfolio.Web/Portfolio.Web.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "Portfolio.Web.dll"]
