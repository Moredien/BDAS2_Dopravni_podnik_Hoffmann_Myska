using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.service;

namespace DopravniPodnik.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    [ObservableProperty]
    private string username;

//passwordbox not yet bound
    
    [RelayCommand]
    void Login()
    {
        //do so authentication
        WindowManager.SetMenuView("LoggedIn");
        Exit();
    }

    [RelayCommand]
    private void Exit()
    {
        WindowManager.SetContentViewToSelected();
    }
}