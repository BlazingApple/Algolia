var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.BlazingApple_Algolia>("blazingapple-algolia");

builder.Build().Run();
