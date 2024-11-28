using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using DopravniPodnik.ViewModels.Menu;

namespace DopravniPodnik.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBase _currentPage;

    [ObservableProperty] 
    private ViewModelBase? _currentMenu;
    
    [ObservableProperty]
    public ListItemTemplate _selectedListItem;

    private GenericGridViewModel GridViewModel;
    // private UserService _userService = new();
    partial void OnSelectedListItemChanged(ListItemTemplate? value)
    {
        if (value == null) 
            value = MenuItems[0];

        WindowManager.SetContentView(typeof(GenericGridViewModel),new object[]{value.ModelType });
    }

    public ObservableCollection<ListItemTemplate> MenuItems { get; } = new();

    public MainWindowViewModel()
    {
        WindowManager.MainWindow = this;

        MenuItems.Add(new ListItemTemplate("UživateleView", typeof(UzivatelDTO),typeof(GenericGridViewModel)));
        MenuItems.Add(new ListItemTemplate("Typy uživatele", typeof(TypyUzivatele),typeof(GenericGridViewModel)));
        MenuItems.Add(new ListItemTemplate("Adresy", typeof(Adresy),typeof(GenericGridViewModel)));
        MenuItems.Add(new ListItemTemplate("Zamestnanci", typeof(Zamestnanci),typeof(GenericGridViewModel)));
        MenuItems.Add(new ListItemTemplate("Typy vozidel", typeof(TypyVozidel),typeof(GenericGridViewModel)));
        MenuItems.Add(new ListItemTemplate("Řidiči", typeof(Ridici),typeof(GenericGridViewModel)));
        MenuItems.Add(new ListItemTemplate("Zastávky", typeof(Zastavky),typeof(GenericGridViewModel)));
        MenuItems.Add(new ListItemTemplate("Typy předplatného", typeof(TypyPredplatneho),typeof(GenericGridViewModel)));
        
        
        SelectedListItem = MenuItems[0];
        
        WindowManager.SetMenuView(typeof(AnonymousUserMenuViewModel));
        CurrentMenu = WindowManager.CurrentMenuViewModel;
    }
}

