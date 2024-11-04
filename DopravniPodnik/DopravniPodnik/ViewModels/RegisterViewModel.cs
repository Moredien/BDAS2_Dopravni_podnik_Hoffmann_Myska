using System.Windows;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.service;

namespace DopravniPodnik.ViewModels;

public partial class RegisterViewModel : ViewModelBase
{
    [RelayCommand]
    private void Exit()
    {
        WindowManager.SetContentViewToSelected();
    }
}