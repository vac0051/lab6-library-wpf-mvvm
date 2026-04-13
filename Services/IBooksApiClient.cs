using Lab6.LibraryClient.Wpf.Models;

namespace Lab6.LibraryClient.Wpf.Services;

public sealed record ApiDebugResult(bool IsSuccess, string Message);

public interface IBooksApiClient
{
    Task<IReadOnlyList<BookDto>> GetBooksAsync();
    Task<bool> CreateBookAsync(BookUpsertDto dto);
    Task<bool> DeleteBookAsync(int id);
    Task<ApiDebugResult> DebugApiAsync();
    Uri? SwaggerUri { get; }
}
