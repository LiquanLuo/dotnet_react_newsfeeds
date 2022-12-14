using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddCors(
    opt => {
        opt.AddPolicy("CorsPolicy", policy => {
            policy.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:3000");
        });
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
// read from appsettings.Development.json
if (app.Environment.IsDevelopment()) 
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var serviceProvider = scope.ServiceProvider;
try
{
    var dataContext = serviceProvider.GetRequiredService<DataContext>();
    dataContext.Database.Migrate();
    await Seed.SeedData(dataContext);
}
catch (Exception ex)
{
    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occur during migration");
}


app.Run();
