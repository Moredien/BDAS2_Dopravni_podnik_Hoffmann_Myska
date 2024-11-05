using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;

namespace DopravniPodnik.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _username = "";

//passwordbox not yet bound
    
    [RelayCommand]
    private void Login()
    {
        //do so authentication
        WindowManager.SetMenuView(ViewType.LoggedInMenu);
        Exit();
    }
}