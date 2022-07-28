using Microsoft.EntityFrameworkCore;
using MovieAPI.Models;

namespace MovieAPI.Data;

public class DatabaseOperations
{
    private MovieContext MovieContext { get; }
    
    private MovieContext GetApplicationMovieContext() => MovieContext;
    public bool IsAnyRegisterOnDatabase()
    {
        var movieContext = GetApplicationMovieContext();
        return movieContext.Movies.Any();
    }
    private void AddMovie(Movie movie)
    {
        var movieContext = GetApplicationMovieContext();
        movieContext.Add(movie);
        movieContext.SaveChanges();
    }
    public void AddListOfMovies(List<Movie> listOfMovies)
    {
        var movieContext = GetApplicationMovieContext();
        movieContext.AddRange(listOfMovies);
        movieContext.SaveChanges();
    }
    
    public DatabaseOperations(MovieContext movieContext)
        => MovieContext = movieContext;
}