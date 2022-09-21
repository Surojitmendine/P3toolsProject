# P3 Prediction
Sales Forecasting, Production Planning, Purchase Plan, Procurement Plan based on forecasting and prediction using Pulling.
**[Entity Framework Core tools reference - .NET CLI ](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet#dotnet-ef-dbcontext-scaffold)** (***dotnet ef dbcontext scaffold***)
```
dotnet ef dbcontext scaffold "Server=SAJAL-PC;user id=sa;password=Rattlesnak3;Database=DB_P3;MultipleActiveResultSets=true" Microsoft.EntityFrameworkCore.SqlServer -o Models --context-dir Context -c DBContext -f --use-database-names
```

**[Configure Nginx](https://docs.microsoft.com/en-us/
