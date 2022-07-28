using Microsoft.EntityFrameworkCore;
using MovieAPI.Data;
using MovieAPI.Models;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddDbContext<MovieContext>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

EnsureDatabaseIsCreated(app);
PopulateDatabaseIfEmpty(app);

app.MapGet("/", Handler);
app.Run();

async ValueTask<List<Movie>> Handler()
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<MovieContext>();
    var completeListOfMovies = await dbContext.Movies.ToListAsync();
    return completeListOfMovies;
}
void EnsureDatabaseIsCreated(WebApplication webApplication)
{
    using var scope = webApplication.Services.CreateScope();
    var services = scope.ServiceProvider;
    var moviesDatabaseContext = services.GetRequiredService<MovieContext>();
    moviesDatabaseContext.Database.EnsureCreated();
}
void PopulateDatabaseIfEmpty(WebApplication webApplication)
{
    using var scope = webApplication.Services.CreateScope();
    var services = scope.ServiceProvider;
    var moviesDatabaseContext = services.GetRequiredService<MovieContext>();
    var databaseSeeder = new DatabaseSeeding(moviesDatabaseContext);
    databaseSeeder.SeedDatabaseWithSampleData();
}    

public partial class Program { }