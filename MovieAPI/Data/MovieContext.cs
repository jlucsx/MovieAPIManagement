using Microsoft.EntityFrameworkCore;
using MovieAPI.Models;

namespace MovieAPI.Data;

public class MovieContext : DbContext
{
    public DbSet<Movie>? Movies { get; set; }
    
    public string DbPath { get; }
    
    public MovieContext()
    {
        var folder = Directory.GetCurrentDirectory();
        var path = System.IO.Path.GetFullPath(folder);
        DbPath = System.IO.Path.Join(path, "movies.db");
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}