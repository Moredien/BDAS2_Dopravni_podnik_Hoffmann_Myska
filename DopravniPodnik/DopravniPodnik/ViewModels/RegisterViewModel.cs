using System.Windows;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.service;

namespace DopravniPodnik.ViewModels;

public partial class RegisterViewModel : ViewModelBase
{
    public event Action ExitRegistationViewAction; 
    
    [RelayCommand]
    private void Exit()
    {
        WindowManager.SetContentViewToSelected();
    }
}