using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;

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
        // WindowManager.SetContentView(value.ModelType,true,null);
        // GridViewModel.SetContent(value.ModelType);
        WindowManager.SetContentView(typeof(GenericGridViewModel),true,value.Key,new object[]{_userService,value.ModelType });
    }

    public ObservableCollection<ListItemTemplate> MenuItems { get; } = new();

    public MainWindowViewModel()
    {
        WindowManager.MainWindow = this;
        // GridViewModel =(GenericGridViewModel) WindowManager.SetContentView(typeof(GenericGridViewModel), true, null);
        // MenuItems.Add(new ListItemTemplate("Uživatelé", typeof(UzivateleViewModel)));
        // MenuItems.Add(new ListItemTemplate("Jízdy", typeof(JizdyViewModel)));
        // MenuItems.Add(new ListItemTemplate("Zastávky", typeof(ZastavkyViewModel)));
        // MenuItems.Add(new ListItemTemplate("Generic Uzivatele", typeof(GenericGridViewModel)));
        MenuItems.Add(new ListItemTemplate("UživateleView", typeof(UzivatelDTO),"GridViewUzivateleDTO"));
        MenuItems.Add(new ListItemTemplate("Typy uživatele", typeof(TypyUzivatele),"GridViewTypyUzivatele"));
        
        
        SelectedListItem = MenuItems[0];
        
        WindowManager.SetMenuView(typeof(AnonymousUserMenuViewModel));
        CurrentMenu = WindowManager.CurrentMenuViewModel;
    }
}

