using CoTuongBackend.API;
using CoTuongBackend.API.Hubs;
using CoTuongBackend.Application;
using CoTuongBackend.Infrastructure;
using CoTuongBackend.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAPIServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.EnableDeepLinking();
    options.EnableFilter();
    options.EnableValidator();
    options.EnableTryItOutByDefault();
    options.EnablePersistAuthorization();
});

if (app.Environment.IsDevelopment())
{

    using var scope = app.Services.CreateScope();
    var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
    await initialiser.InitialiseAsync();
    await initialiser.SeedAsync();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("", () => Results.Redirect("/swagger"))
    .ExcludeFromDescription();

app.MapHub<GameHub>("hubs/game");

app.Run();
