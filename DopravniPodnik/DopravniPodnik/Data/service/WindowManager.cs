using DopravniPodnik.ViewModels;
using DopravniPodnik.ViewModels.Menu;

namespace DopravniPodnik.Data.service;

public static class WindowManager
{ 
    public static MainWindowViewModel? MainWindow {get;set;}
    //viewmodels for the main content
    // private static readonly Dictionary<ViewType, ViewModelBase> ContentViewModels = new();
    private static readonly Dictionary<string, ViewModelBase> ContentViewModels = new();
    //viewmodels for the top menu
    private static readonly Dictionary<Type, ViewModelBase> MenuViewModels = new()
    {
        {typeof(AnonymousUserMenuViewModel),(ViewModelBase)Activator.CreateInstance(typeof(AnonymousUserMenuViewModel))},
        {typeof(LoggedInUserMenuViewModel),(ViewModelBase)Activator.CreateInstance(typeof(LoggedInUserMenuViewModel))}
    };

    public static ViewModelBase? CurrentMenuViewModel { get; set; }

    public static ViewModelBase? CurrentContentViewModel { get; set; }
   
    // adds new content view to a collection for repeated access
    // also sets the first one as selected
    public static void AddNewContentView(ViewModelBase contentViewModel, string key)
    {
        if (!ContentViewModels.TryAdd(key, contentViewModel))
            return;
        if (CurrentContentViewModel == null)
        {
            CurrentContentViewModel = ContentViewModels[key];
        }
    }
/// <summary>
/// changes the content view
/// </summary>
/// <param name="type">type of viewmodel to create</param>
/// <param name="permanent">if true, the view will be saved into dictionary and won't be created again next time</param>
/// <param name="key">Key to under which it's saved in dictionary</param>
/// <param name="parameters">Additional paramaters for viewmodel constructor. usually used to give some context to an edit form</param>
/// <returns></returns>
/// <exception cref="Exception"></exception>
    public static ViewModelBase SetContentView(Type? type,bool permanent,string key, object[] parameters)
    {
        //create new view without saving it. for forms that are always initialized with new data
        if (!permanent)
        {
            if (type == null)
                throw new Exception("Cannot create new view without specifying type");
            CurrentContentViewModel = CreateNewViewInstance(type, parameters);
            MainWindow.CurrentPage = CurrentContentViewModel;
            return CurrentContentViewModel;
        }

        //no key was given >> we are returning to the selected item in the menu
        if (type == null)
        {
            if (MainWindow.SelectedListItem == null)
                throw new Exception("Cannot open selected view when no view is selected");
            type = MainWindow.SelectedListItem.ModelType;
            key = MainWindow.SelectedListItem.Key;
        }
        // the view hasn't been created yet, lets create it
        if (!ContentViewModels.ContainsKey(key))
            ContentViewModels.Add(key,CreateNewViewInstance(type, parameters));
        
        CurrentContentViewModel = ContentViewModels[key];
        MainWindow.CurrentPage = CurrentContentViewModel;
        return CurrentContentViewModel;
    }

    //changes the top menu
    public static void SetMenuView(Type key)
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