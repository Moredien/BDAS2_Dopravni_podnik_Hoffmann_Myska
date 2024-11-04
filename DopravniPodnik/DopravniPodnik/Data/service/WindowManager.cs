using DopravniPodnik.Utils;
using DopravniPodnik.ViewModels;

namespace DopravniPodnik.Data.service;

public static class WindowManager
{ 
    public static MainWindowViewModel? MainWindow {get;set;}
    //viewmodels for the main content
    private static readonly Dictionary<ViewType, ViewModelBase> ContentViewModels = new();
    //viewmodels for the top menu
    private static readonly Dictionary<ViewType, ViewModelBase> MenuViewModels = new()
    {
        {ViewType.AnonymousMenu,(ViewModelBase)Activator.CreateInstance(typeof(AnonymousUserMenuViewModel))},
        {ViewType.LoggedInMenu,(ViewModelBase)Activator.CreateInstance(typeof(LoggedInUserViewModel))}
    };

    public static ViewModelBase? CurrentMenuViewModel { get; set; }

    public static ViewModelBase? CurrentContentViewModel { get; set; }
   

    public static void AddNewContentView(ViewModelBase contentViewModel, ViewType key)
    {
        if (!ContentViewModels.TryAdd(key, contentViewModel))
            return;
        if (CurrentContentViewModel == null)
        {
            CurrentContentViewModel = ContentViewModels[key];
        }
    }

    public static void SetContentView(ViewType key)
    {
        if (ContentViewModels.ContainsKey(key) && MainWindow != null)
        {
            CurrentContentViewModel = ContentViewModels[key];
            MainWindow.CurrentPage = CurrentContentViewModel;
        }
    }

    public static void SetContentViewToSelected()
    {
        if (MainWindow != null && MainWindow?.SelectedListItem != null)
            SetContentView(MainWindow.SelectedListItem.ViewTypeEnum);
        else
            throw new Exception("Failed setting content view to selected");
    }

    public static void SetMenuView(ViewType key)
    {
        if (MainWindow == null)
            throw new Exception("Failed to set menu view. MainWindow is not set");
        if (MenuViewModels.TryGetValue(key, out var model))
        {
            CurrentMenuViewModel = model;
            MainWindow.CurrentMenu = CurrentMenuViewModel;
        }
        else
            throw new Exception($"Failed to set menu view. MenuView {key} doesnt exist.");
        
    }

}