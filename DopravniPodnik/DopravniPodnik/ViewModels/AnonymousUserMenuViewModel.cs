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
        WindowManager.OpenNewFormView(typeof(LoginViewModel), null);
    }
    [RelayCommand]
    private void OpenRegisterView()
    {
        WindowManager.OpenNewFormView(typeof(RegisterViewModel), null);
    }
}