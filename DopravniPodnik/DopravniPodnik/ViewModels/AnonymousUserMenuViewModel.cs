using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;

namespace DopravniPodnik.ViewModels;

public partial class AnonymousUserMenuViewModel : ViewModelBase
{
    [RelayCommand]
    private void OpenLoginView()
    {
        WindowManager.SetContentView(ViewType.Login);
    }
    [RelayCommand]
    private void OpenRegisterView()
    {
        WindowManager.SetContentView(ViewType.Register);
    }
}