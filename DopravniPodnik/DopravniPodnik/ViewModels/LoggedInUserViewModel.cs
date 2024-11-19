using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;

namespace DopravniPodnik.ViewModels;

public partial class LoggedInUserViewModel : ViewModelBase
{
    [RelayCommand]
    private void LogOut()
    {
        //logging out actions can be called from here
        WindowManager.SetMenuView(typeof(AnonymousUserMenuViewModel));
    }
}