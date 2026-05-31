namespace Core;

public class Book
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Author { get; set; }
    public required string Description { get; set; }

    public override string ToString()
    {
        return $"Id: {Id}, Name {Name}, Author: {Author}, Description: {Description}";
    }
}