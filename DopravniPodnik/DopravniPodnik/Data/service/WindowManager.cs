using DopravniPodnik.Utils;
using DopravniPodnik.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
   
    // adds new content view to a collection for repeated access
    // also sets the first one as selected
    public static void AddNewContentView(ViewModelBase contentViewModel, ViewType key)
    {
        if (!ContentViewModels.TryAdd(key, contentViewModel))
            return;
        if (CurrentContentViewModel == null)
        {
            CurrentContentViewModel = ContentViewModels[key];
        }
    }
    //changes the main content view
    public static void SetContentView(ViewType key)
    {
        if (ContentViewModels.ContainsKey(key) && MainWindow != null)
        {
            CurrentContentViewModel = ContentViewModels[key];
            MainWindow.CurrentPage = CurrentContentViewModel;
        }
    }
    // changes the main content to whatever is selected in the side menu
    // used to close one time use views like forms
    public static void SetContentViewToSelected()
    {
        if (MainWindow != null && MainWindow?.SelectedListItem != null)
            SetContentView(MainWindow.SelectedListItem.ViewTypeEnum);
        else
            throw new Exception("Failed setting content view to selected");
    }
    //changes the top menu
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
    // creates new view that is not kept in any collection and not displayed in the side menu
    public static void OpenNewFormView(Type type, object[] parameters)
    {
        var form = (ViewModelBase)Activator.CreateInstance(type,parameters);
        MainWindow.CurrentPage = form;
    }

}