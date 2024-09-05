using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

// https://learn.microsoft.com/en-us/dotnet/aspire/database/sql-server-integration?tabs=dotnet-cli
// https://medium.com/@f.sazanavets/using-sql-database-components-in-net-aspire-c3cee9904138

var password = builder.AddParameter("password", secret: true);

var sql = builder.AddSqlServer("sql", password, port: 3000)
                 .AddDatabase("sqldata", "FIFA");

var api = builder.AddProject<Projects.WebApiFIFA>("backend")
    .WithReference(sql)
    .WithEnvironment("TitleMsg", builder.Configuration["SomeValue"])
    .WithEnvironment("ConnectionStrings:DefaultConnection", builder.Configuration.GetConnectionString("DefaultConnection"));
    
builder.AddProject<Projects.BlazorFIFA>("frontend")
    .WithReference(api);
 
builder.Build().Run();