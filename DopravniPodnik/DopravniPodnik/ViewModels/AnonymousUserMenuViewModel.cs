using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using DopravniPodnik.ViewModels.Forms;

namespace DopravniPodnik.ViewModels;

public partial class AnonymousUserMenuViewModel : ViewModelBase
{
    [RelayCommand]
    private void OpenLoginView()
    {
        WindowManager.SetContentView(typeof(LoginViewModel),false,null);
    }
    [RelayCommand]
    private void OpenRegisterView()
    {
        WindowManager.SetContentView(typeof(RegisterViewModel),false,null);
    }
}