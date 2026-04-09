using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Lab6.LibraryClient.Wpf.Models;

namespace Lab6.LibraryClient.Wpf.Services;

public sealed class BooksApiClient : IBooksApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly IHttpClientFactory _httpClientFactory;

    public BooksApiClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IReadOnlyList<BookDto>> GetBooksAsync()
    {
        var client = _httpClientFactory.CreateClient("LibraryApi");

        var books = await client.GetFromJsonAsync<List<BookDto>>("api/books", JsonOptions);
        return books ?? [];
    }

    public async Task<bool> CreateBookAsync(BookUpsertDto dto)
    {
        var client = _httpClientFactory.CreateClient("LibraryApi");
        using var response = await client.PostAsJsonAsync("api/books", dto, JsonOptions);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("LibraryApi");
        using var response = await client.DeleteAsync($"api/books/{id}");
        return response.IsSuccessStatusCode;
    }
}
