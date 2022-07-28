using MovieAPI.Models;

namespace MovieAPI.Data;

public class DatabaseSeeding
{
    private readonly MovieContext _movieContext;
    public static readonly List<Movie> MoviesForDatabaseSeeding = new()
    {
        new Movie()
        {
            Id = 1,
            Title = "As tranças do Rei Careca",
            Description = "Um filme que nunca soube se existe ou não",
            Author = "Minha mãe"
        },
        new Movie
        {
            Id = 2,
            Title = "O poço e o pendalo",
            Description = "Um filme que sei que existe",
            Author = "Alguém aí"
        },
        new Movie
        {
            Id = 3,
            Title = "SMASH LEGENDS",
            Description = "Isso não é um jogo?! Tem filme também?",
            Author = "Alguma empresa aí"
        },
        new Movie()
        {
            Id = 4,
            Title = "Foo Bar",
            Description = "Fus Ro Dah",
            Author = "Foo? Bar? IDK"
        }
    };

    public void SeedDatabaseWithSampleData()
    {
        DatabaseOperations databaseOperationHandler = new(_movieContext);
        if (databaseOperationHandler.IsAnyRegisterOnDatabase()) 
            return;
        databaseOperationHandler.AddListOfMovies(MoviesForDatabaseSeeding);
    }
    
    public DatabaseSeeding(MovieContext movieContext)
    {
        _movieContext = movieContext;
    }
}