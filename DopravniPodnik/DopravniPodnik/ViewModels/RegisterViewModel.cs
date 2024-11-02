using System.Windows;
using CommunityToolkit.Mvvm.Input;

namespace DopravniPodnik.ViewModels;

public partial class RegisterViewModel : ViewModelBase
{
    public event Action ExitRegistationViewAction; 
    
    [RelayCommand]
    private void Exit()
    {
        var parent = Application.Current.MainWindow?.DataContext as MainWindowViewModel;
        parent?.ChangeViewToDefaultCommand.Execute(parent);
        // ExitRegistationViewAction.Invoke();
    }
}