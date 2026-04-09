using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Lab6.LibraryClient.Wpf.Models;

public sealed class BookDto : INotifyPropertyChanged
{
    private int _id;
    private string _title = string.Empty;
    private int _publicationYear;
    private int _authorId;
    private string _authorName = string.Empty;
    private ObservableCollection<string> _genres = [];

    public int Id
    {
        get => _id;
        set => SetField(ref _id, value);
    }

    public string Title
    {
        get => _title;
        set => SetField(ref _title, value);
    }

    public int PublicationYear
    {
        get => _publicationYear;
        set => SetField(ref _publicationYear, value);
    }

    public int AuthorId
    {
        get => _authorId;
        set => SetField(ref _authorId, value);
    }

    public string AuthorName
    {
        get => _authorName;
        set => SetField(ref _authorName, value);
    }

    public ObservableCollection<string> Genres
    {
        get => _genres;
        set
        {
            if (SetField(ref _genres, value))
            {
                OnPropertyChanged(nameof(GenresText));
            }
        }
    }

    public string GenresText => string.Join(", ", Genres);

    public event PropertyChangedEventHandler? PropertyChanged;

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
