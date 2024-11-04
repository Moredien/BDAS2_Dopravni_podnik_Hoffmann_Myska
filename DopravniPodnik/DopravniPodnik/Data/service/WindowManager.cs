using DopravniPodnik.Utils;
using DopravniPodnik.ViewModels;

namespace DopravniPodnik.Data.service;

public static class WindowManager
{ 
    public static MainWindowViewModel? MainWindow {get;set;}
    //viewmodels for the main content
    private static Dictionary<ViewType, ViewModelBase> contentViewModels = new();
    //viewmodels for the top menu
    private static Dictionary<ViewType, ViewModelBase> menuViewModels = new()
    {
        {ViewType.AnonymousMenu,(ViewModelBase)Activator.CreateInstance(typeof(AnonymousUserMenuViewModel))},
        {ViewType.LoggedInMenu,(ViewModelBase)Activator.CreateInstance(typeof(LoggedInUserViewModel))}
    };

    public static ViewModelBase? CurrentMenuViewModel { get; set; }

    public static ViewModelBase? CurrentContentViewModel { get; set; }
   

    public static void AddNewContentView(ViewModelBase? contentViewModel, ViewType key)
    {
        if (contentViewModels.ContainsKey(key))
            return;
        contentViewModels.Add(key,contentViewModel);
        if (contentViewModels == null)
        {
            CurrentContentViewModel = contentViewModels[key];
        }
    }

    public static void SetContentView(ViewType key)
    {
        if (contentViewModels.ContainsKey(key))
        {
            CurrentContentViewModel = contentViewModels[key];
            MainWindow.CurrentPage = CurrentContentViewModel;
        }
    }

    public static void SetContentViewToSelected()
    {
        if(MainWindow.SelectedListItem!=null)
            SetContentView(MainWindow.SelectedListItem.ViewTypeEnum);
    }

    public static void SetMenuView(ViewType key)
    {
        if (menuViewModels.ContainsKey(key))
        {
            CurrentMenuViewModel = menuViewModels[key];
            MainWindow.CurrentMenu = CurrentMenuViewModel;
        }
        else
        {
            throw new Exception($"Failed to set menuview. MenuView {key} doesnt exist.");
        }
    }

}