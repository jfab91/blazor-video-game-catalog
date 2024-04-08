# Game Store

## Starting SQL Server Docker Container

```bash
$sa_password = "SA PASSWORD HERE"

docker run --cap-add SYS_PTRACE -e "ACCEPT_EULA=1" -e "MSSQL_SA_PASSWORD=$sa_password" -p 1433:1433 --name azuresqledge -d -v sqldata:/var/opt/mssql mcr.microsoft.com/azure-sql-edge
```

## Setting DB Connection String in Secret Manager

```bash
$sa_password = "SA PASSWORD HERE"

dotnet user-secrets set "ConnectionStrings:GameStoreContext" "Server=localhost;Database=GameStore; User Id=sa; Password=$sa_password; TrustServerCertificate=True;"
```

> Secret Manager is intended to be used only on local development environment
