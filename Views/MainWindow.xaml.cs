using System.Windows;
using Lab6.LibraryClient.Wpf.ViewModels;

namespace Lab6.LibraryClient.Wpf.Views;

public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;

        Loaded += (_, _) =>
        {
            if (viewModel.LoadBooksCommand.CanExecute(null))
            {
                viewModel.LoadBooksCommand.Execute(null);
            }
        };
    }
}
