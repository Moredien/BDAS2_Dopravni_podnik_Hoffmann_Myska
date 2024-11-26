﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;

namespace DopravniPodnik.ViewModels.Menu;

public partial class LoggedInUserMenuViewModel : ViewModelBase
{
    [ObservableProperty] 
    private string username;

    [RelayCommand]
    private void LogOut()
    {
        //logging out actions can be called from here
        WindowManager.SetMenuView(typeof(AnonymousUserMenuViewModel));
    }

    public override void Update()
    {
        Username = App.UserSessionInstance.UserName;
    }
}