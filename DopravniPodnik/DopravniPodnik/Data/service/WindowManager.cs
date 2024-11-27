using DopravniPodnik.ViewModels;
using DopravniPodnik.ViewModels.Menu;

namespace DopravniPodnik.Data.service;

public static class WindowManager
{ 
    public static MainWindowViewModel? MainWindow {get;set;}
    //viewmodels for the top menu
    private static readonly Dictionary<Type, ViewModelBase> MenuViewModels = new()
    {
        {typeof(AnonymousUserMenuViewModel),(ViewModelBase)Activator.CreateInstance(typeof(AnonymousUserMenuViewModel))},
        {typeof(LoggedInUserMenuViewModel),(ViewModelBase)Activator.CreateInstance(typeof(LoggedInUserMenuViewModel))}
    };

    public static ViewModelBase? CurrentMenuViewModel { get; set; }

    public static ViewModelBase? CurrentContentViewModel { get; set; }
    
    public static ViewModelBase SetContentView(Type? type, object[] parameters)
    {
        if (type == null)
            throw new Exception("Cannot create new view without specifying type");
        CurrentContentViewModel = CreateNewViewInstance(type, parameters);
        MainWindow.CurrentPage = CurrentContentViewModel;
        return CurrentContentViewModel;
    }

    public static ViewModelBase ReturnToSelectedContentView()
    {
        return SetContentView(MainWindow.SelectedListItem.ViewModelType,new object[]{MainWindow.SelectedListItem.ModelType });
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
            CurrentMenuViewModel.Update();
        }
        else
            throw new Exception($"Failed to set menu view. MenuView {key} doesnt exist.");
        
    }
    private static ViewModelBase CreateNewViewInstance(Type type, object[] parameters)
    {
        // if (parameters == null)
        //     return (ViewModelBase)Activator.CreateInstance(type);
        return (ViewModelBase)Activator.CreateInstance(type, parameters);
    }

}