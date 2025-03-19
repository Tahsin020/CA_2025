var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.CA_2025_WebAPI>("ca-2025-webapi");

builder.Build().Run();
