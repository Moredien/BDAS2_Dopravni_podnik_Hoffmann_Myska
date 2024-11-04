using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.service;

namespace DopravniPodnik.ViewModels;

public partial class AnonymousUserMenuViewModel : ViewModelBase
{
    [RelayCommand]
    private void OpenLoginView()
    {
        WindowManager.SetContentView("Login");
    }
    [RelayCommand]
    private void OpenRegisterView()
    {
        WindowManager.SetContentView("Register");
    }
}