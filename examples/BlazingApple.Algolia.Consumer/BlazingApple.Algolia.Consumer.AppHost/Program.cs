var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.BlazingApple_Algolia_Consumer_ApiService>("apiservice");

builder.AddProject<Projects.BlazingApple_Algolia_Consumer_Web>("webfrontend")
	.WithExternalHttpEndpoints()
	.WithReference(apiService);

builder.Build().Run();
