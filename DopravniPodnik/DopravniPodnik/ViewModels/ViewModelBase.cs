using System.ComponentModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.service;

namespace DopravniPodnik.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    [RelayCommand]
    protected void Exit()
    {
        WindowManager.SetContentView(null,true,null);
    }
    
    
}