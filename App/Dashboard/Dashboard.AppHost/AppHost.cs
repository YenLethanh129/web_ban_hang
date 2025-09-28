var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Dashboard_API>("dashboard-api");

builder.Build().Run();
