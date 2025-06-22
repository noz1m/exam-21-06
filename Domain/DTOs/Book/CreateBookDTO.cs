namespace Domain.DTOs.Book;

public class CreateBookDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int PublishedYear { get; set; }
    public int AuthorId { get; set; }
    public int GenreId { get; set; }
}
