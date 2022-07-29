using MovieAPI.Models;

namespace MovieAPI.Data;

public class DatabaseSeeding
{
    private readonly MovieContext _movieContext;

    private static List<Movie> _moviesForDatabaseSeeding = new()
    {
        new Movie()
        {
            Title = "As tranças do Rei Careca",
            Description = "Um filme que nunca soube se existe ou não",
            Author = "Minha mãe"
        },
        new Movie
        {
            Title = "O poço e o pendalo",
            Description = "Um filme que sei que existe",
            Author = "Alguém aí"
        },
        new Movie
        {
            Title = "SMASH LEGENDS",
            Description = "Isso não é um jogo?! Tem filme também?",
            Author = "Alguma empresa aí"
        },
        new Movie()
        {
            Title = "Foo Bar",
            Description = "Fus Ro Dah",
            Author = "Foo? Bar? IDK"
        }
    };

    public static List<Movie> GetSampleListOfMovies()
    {
        for (var i = 0; i < 4; i++) _moviesForDatabaseSeeding[i].Id = i;
        return _moviesForDatabaseSeeding;
    }
    
    public void SeedDatabaseWithSampleData()
    {
        DatabaseOperations databaseOperationHandler = new(_movieContext);
        if (databaseOperationHandler.IsAnyRegisterOnDatabase()) 
            return;
        databaseOperationHandler.AddListOfMovies(_moviesForDatabaseSeeding);
    }
    
    public DatabaseSeeding(MovieContext movieContext)
    {
        _movieContext = movieContext;
    }
}