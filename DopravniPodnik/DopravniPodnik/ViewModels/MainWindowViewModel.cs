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
    [ObservableProperty] 
    private ViewModelBase _currentPage = new JizdyViewModel();

    [ObservableProperty] 
    private ViewModelBase? _currentMenu;
    
    [ObservableProperty]
    public ListItemTemplate _selectedListItem;

    private GenericGridViewModel GridViewModel;
    private UserService _userService = new();
    partial void OnSelectedListItemChanged(ListItemTemplate? value)
    {
        if (value == null) 
            value = MenuItems[0];

        WindowManager.SetContentView(typeof(GenericGridViewModel),true,value.Key,new object[]{_userService,value.ModelType });
    }

    public ObservableCollection<ListItemTemplate> MenuItems { get; } = new();

    public MainWindowViewModel()
    {
        WindowManager.MainWindow = this;

        MenuItems.Add(new ListItemTemplate("UživateleView", typeof(UzivatelDTO),"GridViewUzivateleDTO"));
        MenuItems.Add(new ListItemTemplate("Typy uživatele", typeof(TypyUzivatele),"GridViewTypyUzivatele"));
        
        
        SelectedListItem = MenuItems[0];
        
        WindowManager.SetMenuView(typeof(AnonymousUserMenuViewModel));
        CurrentMenu = WindowManager.CurrentMenuViewModel;
    }
}

