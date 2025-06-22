namespace Domain.Entities;

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }
    public string Country { get; set; }

    // navigation property
    public List<Book> Books { get; set; }
}
