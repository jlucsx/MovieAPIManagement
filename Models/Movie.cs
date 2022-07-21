namespace MovieAPI.Models;

public record Movie
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Author { get; set; }
}