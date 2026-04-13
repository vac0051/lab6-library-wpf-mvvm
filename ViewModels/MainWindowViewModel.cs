using System.Collections.ObjectModel;
using System.Windows.Input;
using Lab6.LibraryClient.Wpf.Models;
using Lab6.LibraryClient.Wpf.Services;

namespace Lab6.LibraryClient.Wpf.ViewModels;

public sealed class MainWindowViewModel : ViewModelBase
{
    private readonly IBooksApiClient _booksApiClient;
    private string _newTitle = "";
    private int _newPublicationYear = 2020;
    private int _newAuthorId = 1;
    private string _genreIdsInput = "1";
    private string _statusMessage = "Готово";
    private BookDto? _selectedBook;

    public MainWindowViewModel(IBooksApiClient booksApiClient)
    {
        _booksApiClient = booksApiClient;

        LoadBooksCommand = new AsyncRelayCommand(LoadBooksAsync);
        AddBookCommand = new AsyncRelayCommand(AddBookAsync, CanAddBook);
        DeleteBookCommand = new AsyncRelayCommand(DeleteBookAsync, () => SelectedBook is not null);
    }

    public ObservableCollection<BookDto> BooksCollection { get; } = [];

    public BookDto? SelectedBook
    {
        get => _selectedBook;
        set
        {
            if (SetProperty(ref _selectedBook, value))
            {
                (DeleteBookCommand as AsyncRelayCommand)?.RaiseCanExecuteChanged();
            }
        }
    }

    public string NewTitle
    {
        get => _newTitle;
        set
        {
            if (SetProperty(ref _newTitle, value))
            {
                (AddBookCommand as AsyncRelayCommand)?.RaiseCanExecuteChanged();
            }
        }
    }

    public int NewPublicationYear
    {
        get => _newPublicationYear;
        set => SetProperty(ref _newPublicationYear, value);
    }

    public int NewAuthorId
    {
        get => _newAuthorId;
        set => SetProperty(ref _newAuthorId, value);
    }

    public string GenreIdsInput
    {
        get => _genreIdsInput;
        set => SetProperty(ref _genreIdsInput, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public ICommand LoadBooksCommand { get; }
    public ICommand AddBookCommand { get; }
    public ICommand DeleteBookCommand { get; }

    private bool CanAddBook()
    {
        return !string.IsNullOrWhiteSpace(NewTitle);
    }

    private async Task LoadBooksAsync()
    {
        StatusMessage = "Загрузка списка книг...";
        try
        {
            var books = await _booksApiClient.GetBooksAsync();

            BooksCollection.Clear();
            foreach (var book in books)
            {
                BooksCollection.Add(book);
            }

            StatusMessage = $"Загружено книг: {BooksCollection.Count}";
        }
        catch (Exception exception)
        {
            StatusMessage = $"Ошибка загрузки: {exception.Message}";
        }
    }

    private async Task AddBookAsync()
    {
        var genreIds = ParseGenreIds(GenreIdsInput);
        if (genreIds.Count == 0)
        {
            StatusMessage = "Введите хотя бы один Genre ID.";
            return;
        }

        var dto = new BookUpsertDto
        {
            Title = NewTitle.Trim(),
            PublicationYear = NewPublicationYear,
            AuthorId = NewAuthorId,
            GenreIds = genreIds
        };

        try
        {
            var created = await _booksApiClient.CreateBookAsync(dto);
            if (!created)
            {
                StatusMessage = "Не удалось добавить книгу. Проверьте AuthorId и GenreIds.";
                return;
            }

            NewTitle = string.Empty;
            GenreIdsInput = "1";
            await LoadBooksAsync();
            StatusMessage = "Книга добавлена.";
        }
        catch (Exception exception)
        {
            StatusMessage = $"Ошибка добавления: {exception.Message}";
        }
    }

    private async Task DeleteBookAsync()
    {
        if (SelectedBook is null)
        {
            StatusMessage = "Выберите книгу для удаления.";
            return;
        }

        try
        {
            var deleted = await _booksApiClient.DeleteBookAsync(SelectedBook.Id);
            if (!deleted)
            {
                StatusMessage = "Не удалось удалить книгу.";
                return;
            }

            await LoadBooksAsync();
            StatusMessage = "Книга удалена.";
        }
        catch (Exception exception)
        {
            StatusMessage = $"Ошибка удаления: {exception.Message}";
        }
    }

    private static List<int> ParseGenreIds(string input)
    {
        return input
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(part => int.TryParse(part, out var value) ? value : 0)
            .Where(value => value > 0)
            .Distinct()
            .ToList();
    }
}
