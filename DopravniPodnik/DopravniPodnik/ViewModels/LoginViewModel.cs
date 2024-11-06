using System.ComponentModel.DataAnnotations;
using System.Security;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;

namespace DopravniPodnik.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    [ObservableProperty]
    private string uzivatelske_jmeno;
    [ObservableProperty]
    private SecureString heslo;

  


    [RelayCommand]
    private void Login()
    {
        Console.WriteLine($"pw: {PasswordBoxHelper.ConvertToUnsecureString(heslo)}");
        //do so authentication
        WindowManager.SetMenuView(ViewType.LoggedInMenu);
        Exit();
    }
}