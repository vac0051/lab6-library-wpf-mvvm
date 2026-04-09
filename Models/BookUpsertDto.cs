namespace Lab6.LibraryClient.Wpf.Models;

public sealed class BookUpsertDto
{
    public string Title { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public int AuthorId { get; set; }
    public List<int> GenreIds { get; set; } = [];
}
