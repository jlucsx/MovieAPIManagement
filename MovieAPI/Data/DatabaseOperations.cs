using Microsoft.EntityFrameworkCore;
using MovieAPI.Models;

namespace MovieAPI.Data;

public class DatabaseOperations
{
    private MovieContext MovieContext { get; }

    private MovieContext GetApplicationMovieContext() => MovieContext;

    public async ValueTask<long> AddMovie(Movie movie)
    {
        var movieContext = GetApplicationMovieContext();
        var addedMovie = await movieContext.AddAsync(movie);
        await movieContext.SaveChangesAsync();
        return addedMovie.Entity.Id;
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

    public async ValueTask<bool> IsMovieAlreadyOnDatabase(MovieContext movieDbContext, Movie movieFromPostRequest)
    {
        var searchResult = await movieDbContext.Movies.AnyAsync(movie => 
                movie.Title == movieFromPostRequest.Title &&
                movie.Author == movieFromPostRequest.Author &&
                movie.Description == movieFromPostRequest.Description);
        return searchResult;
    }

    public DatabaseOperations(MovieContext movieContext)
        => MovieContext = movieContext;

}