using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Lab6.LibraryClient.Wpf.Models;

namespace Lab6.LibraryClient.Wpf.Services;

public sealed class BooksApiClient : IBooksApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient _httpClient;

    public BooksApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<BookDto>> GetBooksAsync()
    {
        var books = await _httpClient.GetFromJsonAsync<List<BookDto>>("api/books", JsonOptions);
        return books ?? [];
    }

    public async Task<bool> CreateBookAsync(BookUpsertDto dto)
    {
        using var response = await _httpClient.PostAsJsonAsync("api/books", dto, JsonOptions);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        using var response = await _httpClient.DeleteAsync($"api/books/{id}");
        return response.IsSuccessStatusCode;
    }
}
