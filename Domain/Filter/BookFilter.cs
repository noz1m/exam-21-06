namespace Domain.Filter;

public class BookFilter : ValidFilter
{
    public int? GenreId { get; set; }
    public int? AuthorId { get; set; }

    public string? AuthorName { get; set; }
    public string? GenreName { get; set; }
}
