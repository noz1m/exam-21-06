namespace Domain.Entities;

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; }

    // navigation property
    public List<Book> Books { get; set; }
}
