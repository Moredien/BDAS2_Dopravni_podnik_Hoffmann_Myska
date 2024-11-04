using DopravniPodnik.ViewModels;

namespace DopravniPodnik.Data.service;

public static class WindowManager
{ 
    public static MainWindowViewModel? MainWindow {get;set;}
    //viewmodels for the main content
    private static Dictionary<string, ViewModelBase> contentViewModels = new();
    //viewmodels for the top menu
    private static Dictionary<string, ViewModelBase> menuViewModels = new()
    {
        {"Anonymous",(ViewModelBase)Activator.CreateInstance(typeof(AnonymousUserMenuViewModel))},
        {"LoggedIn",(ViewModelBase)Activator.CreateInstance(typeof(LoggedInUserViewModel))}
    };

    public static ViewModelBase currentMenuViewModel { get; set; }

    public static ViewModelBase currentContentViewModel { get; set; }
   

    public static void AddNewContentView(ViewModelBase? contentViewModel, string key)
    {
        if (contentViewModels.ContainsKey(key))
            return;
        contentViewModels.Add(key,contentViewModel);
        if (contentViewModels == null)
        {
            currentContentViewModel = contentViewModels[key];
        }
    }

    public static void SetContentView(string key)
    {
        if (contentViewModels.ContainsKey(key))
        {
            currentContentViewModel = contentViewModels[key];
            MainWindow.CurrentPage = currentContentViewModel;
        }
    }

    public static void SetContentViewToSelected()
    {
        if(MainWindow.SelectedListItem!=null)
            SetContentView(MainWindow.SelectedListItem.Label);
    }

    public static void SetMenuView(string key)
    {
        if (menuViewModels.ContainsKey(key))
        {
            currentMenuViewModel = menuViewModels[key];
            MainWindow.CurrentMenu = currentMenuViewModel;
        }
        else
        {
            throw new Exception($"Failed to set menuview. MenuView {key} doesnt exist.");
        }
    }

}