namespace Domain.Entities;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int PublishedYear { get; set; }
    public int AuthorId { get; set; }
    public int GenreId { get; set; }

        // navigation property
    public Author Author { get; set; }
    public Genre Genre { get; set; }
}
