using System.CodeDom;
using System.Windows.Media.Animation;
using DopravniPodnik.Utils;
using DopravniPodnik.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DopravniPodnik.Data.service;

public static class WindowManager
{ 
    public static MainWindowViewModel? MainWindow {get;set;}
    //viewmodels for the main content
    // private static readonly Dictionary<ViewType, ViewModelBase> ContentViewModels = new();
    private static readonly Dictionary<Type, ViewModelBase> ContentViewModels = new();
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
    public static void AddNewContentView(ViewModelBase contentViewModel, Type key)
    {
        if (!ContentViewModels.TryAdd(key, contentViewModel))
            return;
        if (CurrentContentViewModel == null)
        {
            CurrentContentViewModel = ContentViewModels[key];
        }
    }
    /// <summary>
    /// changes the main content view
    /// </summary>
    /// <param name="key">type of view to be opened</param>
    /// <param name="permanent">if true, it will be saved into dictionary and only initialized once</param>
    /// <param name="parameters">parameters given into the viewmodel constructor</param>
    /// <exception cref="Exception"></exception>
    public static void SetContentView(Type? key,bool permanent, object[] parameters)
    {
        //create new view without saving it. for forms that are always initialized with new data
        if (!permanent)
        {
            if (key == null)
                throw new Exception("Cannot create new view without specifying type");
            CurrentContentViewModel = CreateNewViewInstance(key, parameters);
            MainWindow.CurrentPage = CurrentContentViewModel;
            return;
        }

        //no key was given >> we are returning to the selected item in the menu
        if (key == null)
        {
            if (MainWindow.SelectedListItem == null)
                throw new Exception("Cannot open selected view when no view is selected");
            key = MainWindow.SelectedListItem.ModelType;
        }
        // the view hasn't been created yet, lets create it
        if (!ContentViewModels.ContainsKey(key))
            ContentViewModels.Add(key,CreateNewViewInstance(key, parameters));
        
        CurrentContentViewModel = ContentViewModels[key];
        MainWindow.CurrentPage = CurrentContentViewModel;
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
    private static ViewModelBase CreateNewViewInstance(Type type, object[] parameters)
    {
        return (ViewModelBase)Activator.CreateInstance(type, parameters);
    }

}