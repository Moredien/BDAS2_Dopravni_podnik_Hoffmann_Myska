using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;

namespace DopravniPodnik.ViewModels.Menu;

public partial class AdminMenuViewModel : ViewModelBase
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
        }
    }

    [RelayCommand]
    private void LogOut()
    {
        //logging out actions can be called from here
        UserSession.Instance.EndSession();
        Update();
        WindowManager.SetMenuView(typeof(AnonymousUserMenuViewModel));
    }

    [RelayCommand]
    private void Emulovat()
    {
        Console.WriteLine("emulovat");
    }
    public override void Update()
    {
        Username = App.UserSessionInstance.UserName;
        WindowManager.ChangeUserType(UserSession.Instance.UserType);
    }
}