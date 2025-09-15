# InvestmentTools Desktop

This project uses the .NET Generic Host to wire `EntityFrameworkContext` and check the database connection at startup.

## Run

1. Configure the connection string using [User Secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets):

   ```bash
   dotnet user-secrets set "ConnectionStrings:InvestmentToolsDb" "Server=localhost;Port=3306;Database=investment_tools;User Id=it_user;Password=__PASS__;TreatTinyAsBoolean=true;" --project code/Desktop/src/Desktop.csproj
   ```

2. Start the desktop application:

   ```bash
   dotnet run --project code/Desktop/src/Desktop.csproj
   ```

When the window opens, the title reflects whether the database is reachable.

