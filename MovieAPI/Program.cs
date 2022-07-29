using System.Net;
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

app.MapGet("/api/movies", (MovieContext dbContext) =>
{
    var databaseOperationsHandler = new DatabaseOperations(dbContext);
    var resultContent = databaseOperationsHandler.GetListOfMovies();
    return resultContent.Result.Count == 0 ?
        Results.Ok( Enumerable.Empty<Movie>() ) : 
        Results.Ok(resultContent.Result);
});

app.MapGet("/api/movies/{id:long}", (long id, MovieContext dbContext) =>
{
    var databaseOperationsHandler = new DatabaseOperations(dbContext);
    var resultContent = databaseOperationsHandler.GetMovieWithId(id);
    return Results.Ok(resultContent.Result);
});

app.MapPost("/api/movies/add", (MovieContext dbContext, HttpRequest httpRequest) =>
{
        if (!httpRequest.HasJsonContentType())
            return Results.StatusCode((int)HttpStatusCode.UnsupportedMediaType);
        Movie jsonContent;
        try
        {
            jsonContent = TryToParseReceivedMovie(httpRequest);
        }
        catch (Exception)
        {
            return Results.BadRequest("Could not parse the body of the request according to the provided Content-Type.");
        }
        if (IsAnyPropertyWithIncorrectFormat(jsonContent))
            return Results.BadRequest("Malformed request syntax");
        var databaseOperationsHandler = new DatabaseOperations(dbContext);
        bool isMovieAlreadyOnDatabase = databaseOperationsHandler.IsMovieAlreadyOnDatabase(dbContext, jsonContent).Result;
        if (isMovieAlreadyOnDatabase) return Results.Conflict(
            new {
                Detail = "The movie already exists on the database"
            });
        long addedMovieId;
        try
        {
            addedMovieId = databaseOperationsHandler.AddMovie(jsonContent).Result;
        }
        catch (Exception)
        {
            return Results.StatusCode((int)HttpStatusCode.InternalServerError);
        }

        return Results.Created($"/api/movies/{addedMovieId}", null);
});
    
app.Run();

void EnsureDatabaseIsCreated(WebApplication webApplication)
{
    using var scope = webApplication.Services.CreateScope();
    var services = scope.ServiceProvider;
    var moviesDatabaseContext = services.GetRequiredService<MovieContext>();
    moviesDatabaseContext.Database.EnsureCreated();
}

bool IsAnyPropertyWithIncorrectFormat(Movie deserializedJsonFromPostRequest)
{
    return deserializedJsonFromPostRequest.Title == null ||
           deserializedJsonFromPostRequest.Author == null ||
           deserializedJsonFromPostRequest.Description == null;
}

Movie TryToParseReceivedMovie(HttpRequest httpRequest)
{
    ValueTask<Movie> json = httpRequest.ReadFromJsonAsync<Movie>()!;
    var movie = json.Result!;
    return movie;
}

public partial class Program { }