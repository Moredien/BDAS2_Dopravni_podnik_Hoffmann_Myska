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
        // if (value == null) 
        //     value = MenuItems[0];
        if (MenuItems.Count == 0)
            return;
        if(value.ViewModelType==typeof(GenericGridViewModel))
            WindowManager.SetContentView(value.ViewModelType,new object[]{value.ModelType });
        else
            WindowManager.SetContentView(value.ViewModelType,new object[]{});

    }

    public ObservableCollection<ListItemTemplate> MenuItems { get; } = new();

    public MainWindowViewModel()
    {
        WindowManager.MainWindow = this;

        MenuItems.Add(new ListItemTemplate("Vyhledání cesty", null,typeof(VyhledaniCestyViewModel)));
        MenuItems.Add(new ListItemTemplate("Info o zastávce", null,typeof(InfoOZastavceViewModel)));
        SelectedListItem = MenuItems[0];
        
        WindowManager.SetMenuView(typeof(AnonymousUserMenuViewModel));
        CurrentMenu = WindowManager.CurrentMenuViewModel;
    }
}

