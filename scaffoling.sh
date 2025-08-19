#!/bin/bash

CONNECTION_STRING="Server=localhost,1433;Database=webbanhang;User Id=sa;Password=sa_password@123;TrustServerCertificate=True;"

dotnet ef dbcontext scaffold \
  "$CONNECTION_STRING" \
  Microsoft.EntityFrameworkCore.SqlServer \
  --project App/Dashboard/Dashboard.DataAccess \
  --startup-project App/Dashboard/Dashboard.API \
  --output-dir Models/Entities \
  --context-dir Context \
  --context WebbanhangDbContext \
  --data-annotations \
  --use-database-names \
  --no-onconfiguring \
  --force
 
# dotnet ef migrations add Init --project .\Dashboard.DataAccess\ --startup-project .\Dashboard.API\ --output-dir Data/Migrations
# dotnet ef database update --project .\Dashboard.DataAccess\ --startup-project .\Dashboard.API\
dotnet ef migrations add UpdateOrder --project .\Dashboard.DataAccess\ --startup-project .\Dashboard.API\ --output-dir Data/Migrations