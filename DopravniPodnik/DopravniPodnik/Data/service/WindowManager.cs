using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Utils;
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
        {typeof(LoggedInUserMenuViewModel),(ViewModelBase)Activator.CreateInstance(typeof(LoggedInUserMenuViewModel))},
        {typeof(AdminMenuViewModel),(ViewModelBase)Activator.CreateInstance(typeof(AdminMenuViewModel))}
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
        if(MainWindow.SelectedListItem.ViewModelType== typeof(GenericGridViewModel))
            return SetContentView(MainWindow.SelectedListItem.ViewModelType,new object[]{MainWindow.SelectedListItem.ModelType });
        else
            return SetContentView(MainWindow.SelectedListItem.ViewModelType,new object[]{});
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

    public static void ChangeUserType(TypyUzivatele userType)
    {
        MainWindow.MenuItems.Clear();
        if (userType == null)
        {
            MainWindow.MenuItems.Add(new ListItemTemplate("Vyhledání cesty", null,typeof(VyhledaniCestyViewModel)));
            MainWindow.MenuItems.Add(new ListItemTemplate("Info o zastávce", null,typeof(InfoOZastavceViewModel)));
            return;
        }
        
        switch (userType.Nazev)
        {
            case "Zákazník":
                MainWindow.MenuItems.Add(new ListItemTemplate("Vyhledání cesty", null,typeof(VyhledaniCestyViewModel)));
                MainWindow.MenuItems.Add(new ListItemTemplate("Info o zastávce", null,typeof(InfoOZastavceViewModel)));
                MainWindow.MenuItems.Add(new ListItemTemplate("Profil", typeof(ProfilDTO),typeof(ProfilViewModel)));
                MainWindow.MenuItems.Add(new ListItemTemplate("Karty", typeof(KartyDTO),typeof(KartyViewModel)));
                MainWindow.MenuItems.Add(new ListItemTemplate("Historie Plateb", typeof(HistoriePlatebDTO),typeof(HistoriePlatebViewModel)));
                break;
            case "Zaměstnanec":
                MainWindow.MenuItems.Add(new ListItemTemplate("Info o zastávce",null,typeof(InfoOZastavceViewModel)));
                MainWindow.MenuItems.Add(new ListItemTemplate("Vyhledání cesty", null,typeof(VyhledaniCestyViewModel)));
                MainWindow.MenuItems.Add(new ListItemTemplate("Jízdy", typeof(JizdyDTO),typeof(JizdyViewModel)));
                MainWindow.MenuItems.Add(new ListItemTemplate("Vozový park", typeof(VozovyParkDTO),typeof(VozovyParkViewModel)));
                MainWindow.MenuItems.Add(new ListItemTemplate("Uživatelé", null,typeof(ZakazniciViewModel)));
                break;
            case "Admin":
                MainWindow.MenuItems.Add(new ListItemTemplate("Vyhledání cesty", null,typeof(VyhledaniCestyViewModel)));
                MainWindow.MenuItems.Add(new ListItemTemplate("Info o zastávce", null,typeof(InfoOZastavceViewModel)));
                MainWindow.MenuItems.Add(new ListItemTemplate("Uživatelé", typeof(UzivatelDTO),typeof(GenericGridViewModel)));
                MainWindow.MenuItems.Add(new ListItemTemplate("Typy uživatele", typeof(TypyUzivatele),typeof(GenericGridViewModel)));
                MainWindow.MenuItems.Add(new ListItemTemplate("Adresy", typeof(Adresy),typeof(GenericGridViewModel)));
                MainWindow.MenuItems.Add(new ListItemTemplate("Zamestnanci", typeof(Zamestnanci),typeof(GenericGridViewModel)));
                MainWindow.MenuItems.Add(new ListItemTemplate("Typy vozidel", typeof(TypyVozidel),typeof(GenericGridViewModel)));
                MainWindow.MenuItems.Add(new ListItemTemplate("Řidiči", typeof(Ridici),typeof(GenericGridViewModel)));
                MainWindow.MenuItems.Add(new ListItemTemplate("Zastávky", typeof(Zastavky),typeof(GenericGridViewModel)));
                MainWindow.MenuItems.Add(new ListItemTemplate("Typy předplatného", typeof(TypyPredplatneho),typeof(GenericGridViewModel)));
                MainWindow.MenuItems.Add(new ListItemTemplate("Vozový park", typeof(VozovyParkDTO),typeof(VozovyParkViewModel)));
                MainWindow.MenuItems.Add(new ListItemTemplate("Logy", typeof(Logy),typeof(GenericGridViewModel)));
                MainWindow.MenuItems.Add(new ListItemTemplate("Uživatelé", null,typeof(ZakazniciViewModel)));
                break;
        }

        MainWindow.SelectedListItem = MainWindow.MenuItems[0];
    }

}