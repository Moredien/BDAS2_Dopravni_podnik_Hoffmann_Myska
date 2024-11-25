﻿using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.service;
using DopravniPodnik.ViewModels.Forms;

namespace DopravniPodnik.ViewModels.Menu;

public partial class AnonymousUserMenuViewModel : ViewModelBase
{
    [RelayCommand]
    private void OpenLoginView()
    {
        WindowManager.SetContentView(typeof(LoginViewModel),false,"Login",null);
    }
    [RelayCommand]
    private void OpenRegisterView()
    {
        WindowManager.SetContentView(typeof(RegisterViewModel),false,"Register",null);
    }
}