using Lab6.LibraryClient.Wpf.Models;

namespace Lab6.LibraryClient.Wpf.Services;

public interface IBooksApiClient
{
    Task<IReadOnlyList<BookDto>> GetBooksAsync();
    Task<bool> CreateBookAsync(BookUpsertDto dto);
    Task<bool> DeleteBookAsync(int id);
}
