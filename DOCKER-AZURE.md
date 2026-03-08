# Docker & Azure Deployment Guide

This guide covers building Docker images locally, running them with Docker Compose, and publishing both **Portfolio.Api** and **Portfolio.Web** to Azure using Azure Container Apps or Azure App Service.

---

## Local Docker

### Build images

Build both images from the repo root (the Docker context is `./src`):

```bash
# Build the API image
docker build -t portfolio-api -f src/Portfolio.Api/Dockerfile ./src

# Build the Web image
docker build -t portfolio-web -f src/Portfolio.Web/Dockerfile ./src
```

### Run with Docker Compose

```bash
# Start both containers (builds images first if needed)
docker compose up --build -d

# View running containers
docker compose ps

# View logs
docker compose logs -f

# Stop containers (data volumes preserved)
docker compose down

# Stop and remove all data volumes (destructive)
docker compose down -v
```

The compose file exposes:
- Portfolio.Api  → `http://localhost:5008`
- Portfolio.Web  → `http://localhost:5072`

### Environment variables

Create a `.env` file at the repo root to override defaults:

```env
JWT_KEY=<your-long-random-secret-32-chars-min>
ADMIN_EMAIL=admin@yourdomain.com
ADMIN_PASSWORD=YourSecureP@ssword1!
ALLOWED_ORIGINS=http://localhost:5072
BASE_API_URL=http://localhost:5008/
```

---

## Azure — Prerequisites

Install or update the Azure CLI:

```bash
# Install (macOS/Linux)
curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash

# Update
az upgrade

# Log in
az login

# Set your subscription
az account set --subscription "<subscription-name-or-id>"
```

Install the Azure Container Apps extension:

```bash
az extension add --name containerapp --upgrade
```

---

## Option A — Azure Container Apps (recommended)

Azure Container Apps is the simplest zero-infrastructure path for containerised ASP.NET Core apps.

### 1. Create resource group and environment

```bash
RESOURCE_GROUP="portfolio-rg"
LOCATION="uksouth"
ENVIRONMENT="portfolio-env"

az group create \
    --name $RESOURCE_GROUP \
    --location $LOCATION

az containerapp env create \
    --name $ENVIRONMENT \
    --resource-group $RESOURCE_GROUP \
    --location $LOCATION
```

### 2. Create Azure Container Registry (ACR)

```bash
ACR_NAME="portfolioacr$RANDOM"   # must be globally unique

az acr create \
    --name $ACR_NAME \
    --resource-group $RESOURCE_GROUP \
    --sku Basic \
    --admin-enabled true

# Log Docker in to ACR
az acr login --name $ACR_NAME
```

### 3. Build and push images to ACR

```bash
ACR_LOGIN_SERVER=$(az acr show --name $ACR_NAME --query loginServer -o tsv)

# Build and push Portfolio.Api
docker build -t $ACR_LOGIN_SERVER/portfolio-api:latest -f src/Portfolio.Api/Dockerfile ./src
docker push $ACR_LOGIN_SERVER/portfolio-api:latest

# Build and push Portfolio.Web
docker build -t $ACR_LOGIN_SERVER/portfolio-web:latest -f src/Portfolio.Web/Dockerfile ./src
docker push $ACR_LOGIN_SERVER/portfolio-web:latest
```

### 4. Deploy Portfolio.Api Container App

```bash
ACR_PASSWORD=$(az acr credential show --name $ACR_NAME --query passwords[0].value -o tsv)
JWT_KEY="<your-long-random-jwt-secret>"

az containerapp create \
    --name portfolio-api \
    --resource-group $RESOURCE_GROUP \
    --environment $ENVIRONMENT \
    --image $ACR_LOGIN_SERVER/portfolio-api:latest \
    --registry-server $ACR_LOGIN_SERVER \
    --registry-username $ACR_NAME \
    --registry-password $ACR_PASSWORD \
    --target-port 8080 \
    --ingress external \
    --min-replicas 1 \
    --max-replicas 3 \
    --env-vars \
        DatabaseProvider=Sqlite \
        "ConnectionStrings__DefaultConnection=Data Source=/app/data/portfolio-api.db" \
        SeedData=true \
        Jwt__Key="$JWT_KEY" \
        Jwt__Issuer=Portfolio.Api \
        Jwt__Audience=Portfolio.Web \
        "DefaultAdmin__Email=admin@yourdomain.com" \
        "DefaultAdmin__Password=YourSecureP@ssword1!" \
        AllowedOrigins=https://portfolio-web.<unique>.azurecontainerapps.io
```

Get the API URL:

```bash
API_URL=$(az containerapp show \
    --name portfolio-api \
    --resource-group $RESOURCE_GROUP \
    --query properties.configuration.ingress.fqdn -o tsv)

echo "API URL: https://$API_URL"
```

### 5. Deploy Portfolio.Web Container App

```bash
az containerapp create \
    --name portfolio-web \
    --resource-group $RESOURCE_GROUP \
    --environment $ENVIRONMENT \
    --image $ACR_LOGIN_SERVER/portfolio-web:latest \
    --registry-server $ACR_LOGIN_SERVER \
    --registry-username $ACR_NAME \
    --registry-password $ACR_PASSWORD \
    --target-port 8080 \
    --ingress external \
    --min-replicas 1 \
    --max-replicas 3 \
    --env-vars \
        DatabaseProvider=Sqlite \
        "ConnectionStrings__DefaultConnection=Data Source=/app/data/portfolio-web.db" \
        SeedData=true \
        "BaseApiUrl=https://$API_URL/" \
        "DefaultAdmin__Email=admin@yourdomain.com" \
        "DefaultAdmin__Password=YourSecureP@ssword1!"
```

Get the Web URL:

```bash
WEB_URL=$(az containerapp show \
    --name portfolio-web \
    --resource-group $RESOURCE_GROUP \
    --query properties.configuration.ingress.fqdn -o tsv)

echo "Web URL: https://$WEB_URL"
```

### 6. Update API AllowedOrigins after Web is deployed

```bash
az containerapp update \
    --name portfolio-api \
    --resource-group $RESOURCE_GROUP \
    --set-env-vars "AllowedOrigins=https://$WEB_URL"
```

### 7. Configure the Web → API connection

Sign in to the admin panel at `https://<web-url>/login` and navigate to **Settings**. Set **Portfolio API Base URL** to `https://<api-url>`.

---

## Option B — Azure App Service (Web Apps for Containers)

Use this if you prefer a PaaS approach with built-in SSL, custom domains, and deployment slots.

### 1. Create App Service Plan

```bash
RESOURCE_GROUP="portfolio-rg"
LOCATION="uksouth"
PLAN_NAME="portfolio-plan"

az appservice plan create \
    --name $PLAN_NAME \
    --resource-group $RESOURCE_GROUP \
    --location $LOCATION \
    --is-linux \
    --sku B2
```

### 2. Create Web Apps

```bash
ACR_LOGIN_SERVER="<your-acr>.azurecr.io"

# API Web App
az webapp create \
    --name portfolio-api-app \
    --resource-group $RESOURCE_GROUP \
    --plan $PLAN_NAME \
    --deployment-container-image-name $ACR_LOGIN_SERVER/portfolio-api:latest

# Web Web App
az webapp create \
    --name portfolio-web-app \
    --resource-group $RESOURCE_GROUP \
    --plan $PLAN_NAME \
    --deployment-container-image-name $ACR_LOGIN_SERVER/portfolio-web:latest
```

### 3. Configure ACR credentials

```bash
ACR_NAME="<your-acr-name>"
ACR_PASSWORD=$(az acr credential show --name $ACR_NAME --query passwords[0].value -o tsv)

for APP in portfolio-api-app portfolio-web-app; do
    az webapp config container set \
        --name $APP \
        --resource-group $RESOURCE_GROUP \
        --docker-registry-server-url https://$ACR_LOGIN_SERVER \
        --docker-registry-server-user $ACR_NAME \
        --docker-registry-server-password $ACR_PASSWORD
done
```

### 4. Set app settings (environment variables)

```bash
JWT_KEY="<your-long-random-jwt-secret>"

# Portfolio.Api settings
az webapp config appsettings set \
    --name portfolio-api-app \
    --resource-group $RESOURCE_GROUP \
    --settings \
        DatabaseProvider=Sqlite \
        "ConnectionStrings__DefaultConnection=Data Source=/app/data/portfolio-api.db" \
        SeedData=true \
        Jwt__Key="$JWT_KEY" \
        Jwt__Issuer=Portfolio.Api \
        Jwt__Audience=Portfolio.Web \
        "DefaultAdmin__Email=admin@yourdomain.com" \
        "DefaultAdmin__Password=YourSecureP@ssword1!" \
        AllowedOrigins=https://portfolio-web-app.azurewebsites.net

# Portfolio.Web settings
az webapp config appsettings set \
    --name portfolio-web-app \
    --resource-group $RESOURCE_GROUP \
    --settings \
        DatabaseProvider=Sqlite \
        "ConnectionStrings__DefaultConnection=Data Source=/app/data/portfolio-web.db" \
        SeedData=true \
        "BaseApiUrl=https://portfolio-api-app.azurewebsites.net/" \
        "DefaultAdmin__Email=admin@yourdomain.com" \
        "DefaultAdmin__Password=YourSecureP@ssword1!"
```

### 5. Enable always-on and restart

```bash
for APP in portfolio-api-app portfolio-web-app; do
    az webapp config set \
        --name $APP \
        --resource-group $RESOURCE_GROUP \
        --always-on true

    az webapp restart --name $APP --resource-group $RESOURCE_GROUP
done
```

Get the URLs:

```bash
az webapp show --name portfolio-api-app --resource-group $RESOURCE_GROUP --query defaultHostName -o tsv
az webapp show --name portfolio-web-app --resource-group $RESOURCE_GROUP --query defaultHostName -o tsv
```

---

## Updating a deployment

After pushing code changes, rebuild and push the updated image, then restart the container:

### Container Apps

```bash
# Rebuild and push
docker build -t $ACR_LOGIN_SERVER/portfolio-api:latest -f src/Portfolio.Api/Dockerfile ./src
docker push $ACR_LOGIN_SERVER/portfolio-api:latest

# Update the container app (triggers re-pull)
az containerapp update \
    --name portfolio-api \
    --resource-group $RESOURCE_GROUP \
    --image $ACR_LOGIN_SERVER/portfolio-api:latest
```

### App Service

```bash
# Rebuild and push
docker build -t $ACR_LOGIN_SERVER/portfolio-api:latest -f src/Portfolio.Api/Dockerfile ./src
docker push $ACR_LOGIN_SERVER/portfolio-api:latest

# Restart to pull the new image
az webapp restart --name portfolio-api-app --resource-group $RESOURCE_GROUP
```

---

## Using Azure SQL or PostgreSQL in Azure

Instead of SQLite (which is not recommended for production), switch to a managed database:

### Azure SQL (SQL Server)

```bash
az sql server create \
    --name portfolio-sqlserver \
    --resource-group $RESOURCE_GROUP \
    --location $LOCATION \
    --admin-user sqladmin \
    --admin-password "YourSecureP@ssword1!"

az sql db create \
    --name PortfolioApi \
    --resource-group $RESOURCE_GROUP \
    --server portfolio-sqlserver \
    --service-objective S0

# Allow Azure services to connect
az sql server firewall-rule create \
    --name AllowAzureServices \
    --resource-group $RESOURCE_GROUP \
    --server portfolio-sqlserver \
    --start-ip-address 0.0.0.0 \
    --end-ip-address 0.0.0.0
```

Set the app settings to use SQL Server:

```bash
az containerapp update \
    --name portfolio-api \
    --resource-group $RESOURCE_GROUP \
    --set-env-vars \
        DatabaseProvider=SqlServer \
        "ConnectionStrings__DefaultConnection=Server=portfolio-sqlserver.database.windows.net;Database=PortfolioApi;User=sqladmin;Password=YourSecureP@ssword1!"
```

### Azure Database for PostgreSQL

```bash
az postgres flexible-server create \
    --name portfolio-postgres \
    --resource-group $RESOURCE_GROUP \
    --location $LOCATION \
    --admin-user pgadmin \
    --admin-password "YourSecureP@ssword1!" \
    --sku-name Standard_B1ms \
    --tier Burstable \
    --public-access 0.0.0.0

az postgres flexible-server db create \
    --server-name portfolio-postgres \
    --resource-group $RESOURCE_GROUP \
    --database-name PortfolioApi
```

Set the app settings to use PostgreSQL:

```bash
az containerapp update \
    --name portfolio-api \
    --resource-group $RESOURCE_GROUP \
    --set-env-vars \
        DatabaseProvider=PostgreSql \
        "ConnectionStrings__DefaultConnection=Host=portfolio-postgres.postgres.database.azure.com;Database=PortfolioApi;Username=pgadmin;Password=YourSecureP@ssword1!;SslMode=Require"
```

---

## Clean up all resources

```bash
az group delete --name $RESOURCE_GROUP --yes --no-wait
```

---

## Notes

- **Migrations run automatically** at startup via `Database.MigrateAsync()`. No manual migration steps are needed after deploying a new image.
- **SQLite is not recommended for production** Azure deployments. App Service and Container Apps file systems are ephemeral and may not persist `/app/data`. Use Azure SQL or Azure Database for PostgreSQL instead.
- **Secrets**: Never commit `JWT_KEY`, database passwords, or admin credentials. Use Azure Key Vault or environment variables set via the Azure portal/CLI.
- See **eftooling.txt** for the full list of EF Core CLI commands.
- See **MIGRATIONS.md** for a narrative guide to the migration workflow.
