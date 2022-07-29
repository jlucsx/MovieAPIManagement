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
    public int AddMovie(Movie movie)
    {
        var movieContext = GetApplicationMovieContext();
        movieContext.Add(movie);
        return movieContext.SaveChanges();
    }
    public void AddListOfMovies(List<Movie> listOfMovies)
    {
        var movieContext = GetApplicationMovieContext();
        movieContext.AddRange(listOfMovies);
        movieContext.SaveChanges();
    }
    public async ValueTask<List<Movie>> GetListOfMovies()
    {
        var movieContext = GetApplicationMovieContext();
        var completeListOfMovies = await movieContext.Movies.ToListAsync();
        return completeListOfMovies;
    }
    
    public async ValueTask<Movie?> GetMovieWithId(long id)
    {
        var movieContext = GetApplicationMovieContext();
        var idSpecifiedMovie = await movieContext.Movies.FirstOrDefaultAsync(movie => movie.Id == id);
        return idSpecifiedMovie;
    }
    
    public DatabaseOperations(MovieContext movieContext)
        => MovieContext = movieContext;

}