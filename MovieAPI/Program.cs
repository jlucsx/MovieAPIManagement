using System.Net;
using MovieAPI.Data;
using MovieAPI.Models;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddCors();
builder.Services.AddDbContext<MovieContext>();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseCors(corsServiceOptions => {
    corsServiceOptions.AllowAnyHeader();
    corsServiceOptions.AllowAnyMethod();
    corsServiceOptions.AllowAnyOrigin();
});
app.UseRouting();
app.UseHttpsRedirection();
app.UseFileServer();

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
    if (isMovieAlreadyOnDatabase)
        return Results.Conflict("The movie already exists on the database");
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
    return deserializedJsonFromPostRequest.Title == string.Empty ||
           deserializedJsonFromPostRequest.Author == string.Empty ||
           deserializedJsonFromPostRequest.Description == string.Empty;
}

Movie TryToParseReceivedMovie(HttpRequest httpRequest)
{
    ValueTask<Movie> json = httpRequest.ReadFromJsonAsync<Movie>()!;
    var movie = json.Result!;
    return movie;
}
