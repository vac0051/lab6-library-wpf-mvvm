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

    public Uri? SwaggerUri => _httpClient.BaseAddress is null
        ? null
        : new Uri(_httpClient.BaseAddress, "swagger/index.html");

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

    public async Task<ApiDebugResult> DebugApiAsync()
    {
        try
        {
            using var swaggerResponse = await _httpClient.GetAsync("swagger/v1/swagger.json");
            using var booksResponse = await _httpClient.GetAsync("api/books");

            var isSuccess = swaggerResponse.IsSuccessStatusCode && booksResponse.IsSuccessStatusCode;
            var message = $"Swagger: {(int)swaggerResponse.StatusCode}, Books: {(int)booksResponse.StatusCode}";

            return new ApiDebugResult(isSuccess, message);
        }
        catch (Exception exception)
        {
            return new ApiDebugResult(false, $"HTTP error: {exception.Message}");
        }
    }
}
