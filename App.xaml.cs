using System.Net.Http;
using System.Windows;
using Lab6.LibraryClient.Wpf.Services;
using Lab6.LibraryClient.Wpf.ViewModels;
using Lab6.LibraryClient.Wpf.Views;

namespace Lab6.LibraryClient.Wpf;

public partial class App : Application
{
    private HttpClient? _httpClient;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };

        _httpClient = new HttpClient(handler)
        {
            // Здесь можно прописать HTTP или HTTPS, сертификат проверяться не будет
            BaseAddress = new Uri("http://localhost:5262/") 
        };

        IBooksApiClient apiClient = new BooksApiClient(_httpClient);
        var viewModel = new MainWindowViewModel(apiClient);
        var mainWindow = new MainWindow(viewModel);

        mainWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _httpClient?.Dispose();
        base.OnExit(e);
    }
}
