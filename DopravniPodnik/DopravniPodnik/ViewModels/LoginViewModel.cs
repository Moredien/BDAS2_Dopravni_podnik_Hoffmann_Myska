using System.Windows;
using CommunityToolkit.Mvvm.Input;

namespace DopravniPodnik.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    [RelayCommand]
    void Login()
    {
        //do something
        Exit();
    }

    [RelayCommand]
    private void Exit()
    {
        var parent = Application.Current.MainWindow?.DataContext as MainWindowViewModel;
        parent?.ChangeViewToDefaultCommand.Execute(parent);
    }
}