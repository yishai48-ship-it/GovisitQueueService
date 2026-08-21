using System.Reflection;
using GovisitQueueService.Data;
using Mongo2Go;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Swagger - generated on every run, with the XML doc comments as descriptions.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Govisit Queue Management Service",
        Version = "v1",
        Description = "Service for managing appointment queues (view / update / delete)."
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// Embedded MongoDB: starts a real mongod process on app startup, so the service
// runs out of the box with no separate MongoDB installation.
// To use an external MongoDB instead, drop these two lines and bind the section directly.
var mongoRunner = MongoDbRunner.Start();

builder.Services.Configure<MongoDbSettings>(options =>
{
    builder.Configuration.GetSection("MongoDbSettings").Bind(options);
    options.ConnectionString = mongoRunner.ConnectionString;
});
builder.Services.AddSingleton<MongoDbContext>();

// CQRS via MediatR - scans this assembly for all command/query handlers.
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();

// Stop the embedded MongoDB process when the app shuts down.
app.Lifetime.ApplicationStopping.Register(() => mongoRunner.Dispose());

// Dev-only fixed seed data (see appsettings.Development.json -> SeedData:Appointments),
// so there's something to test against right after "dotnet run". Never runs outside Development.
if (app.Environment.IsDevelopment())
{
    using var seedScope = app.Services.CreateScope();
    var seedContext = seedScope.ServiceProvider.GetRequiredService<MongoDbContext>();
    await DataSeeder.SeedAsync(seedContext, app.Configuration);
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Govisit Queue Management Service v1");
});

app.MapControllers();

app.Run();
