using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using MovieAPI.Data;
using MovieAPI.Models;

namespace APITests;

public class MovieListModel
{
    private readonly List<Movie> _moviesFromDatabaseSeeding = DatabaseSeeding.MoviesForDatabaseSeeding;
    
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };
    public string GetSeedMovieListInJsonFormat()
    {
        return JsonSerializer.Serialize(_moviesFromDatabaseSeeding, _jsonSerializerOptions);
    }

    public string GetIdSpecifiedSeedMovieInJsonFormat(long id)
    {
        return JsonSerializer.Serialize(
            _moviesFromDatabaseSeeding.FirstOrDefault(movie => movie.Id == id),
            _jsonSerializerOptions);
    }
}