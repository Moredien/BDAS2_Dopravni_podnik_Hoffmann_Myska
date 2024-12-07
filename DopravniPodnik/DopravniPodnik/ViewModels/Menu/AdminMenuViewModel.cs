using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;

namespace DopravniPodnik.ViewModels.Menu;

public partial class AdminMenuViewModel : ViewModelBase
{
    [ObservableProperty] 
    private string username;

    private bool safeMode = false;
    [ObservableProperty] public Visibility emulateBtnVisibility = Visibility.Hidden;
    [ObservableProperty] public string emulateBtnText = "Emulovat";
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

    [RelayCommand]
    private void Emulovat()
    {
        if (UserSession.Instance.UserType.Nazev != "Admin")
        {
            UserSession.Instance.EmulateUser(null);
            EmulateBtnText = "Emulovat";
            Update();
        }
            
            
        ViewModelBase vm = WindowManager.CurrentContentViewModel!;
        if (vm.GetType() == typeof(GenericGridViewModel) &&
            ((GenericGridViewModel)vm)?.SelectedItem?.GetType() == typeof(UzivatelDTO))
        {
            var uzivatel = (UzivatelDTO)((GenericGridViewModel)vm).SelectedItem;
            UserSession.Instance.EmulateUser(uzivatel);
            EmulateBtnText = "Ukončit emulaci";
            Update();
        }
    }
    
    public override void Update()
    {
        Username = App.UserSessionInstance.UserName;
        WindowManager.ChangeUserType(UserSession.Instance.UserType);
    }
}