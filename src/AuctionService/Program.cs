using AuctionService.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AuctionDbContext>(options =>
{
  options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

try
{
  DbInitializer.InitDb(app);
}
catch (Exception ex)
{
  var logger = app.Services.GetRequiredService<ILogger<Program>>();
  logger.LogError(ex, "An error occurred while migrating or initializing the database.");
}

app.Run();
