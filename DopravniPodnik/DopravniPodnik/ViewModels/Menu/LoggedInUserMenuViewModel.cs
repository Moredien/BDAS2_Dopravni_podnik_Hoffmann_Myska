﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;

namespace DopravniPodnik.ViewModels.Menu;

public partial class LoggedInUserMenuViewModel : ViewModelBase
{
    [ObservableProperty] 
    private string username;

    private bool safeMode = false;
    public bool SafeMode
    {
        get { return safeMode; }
        set
        {
            safeMode = value;
            UserSession.Instance.IsSafeModeOn = value;
            WindowManager.CurrentContentViewModel.Update();
        }
    }

    [RelayCommand]
    private void LogOut()
    {
        UserSession.Instance.EndSession();
        Update();
        WindowManager.SetMenuView(typeof(AnonymousUserMenuViewModel));
    }

    public override void Update()
    {
        Username = App.UserSessionInstance.UserName;
        WindowManager.ChangeUserType(UserSession.Instance.UserType);
    }
}