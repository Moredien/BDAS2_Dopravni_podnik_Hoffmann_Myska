using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    partial void OnSelectedListItemChanged(ListItemTemplate? value)
    {
        if (value == null) 
            value = MenuItems[0];
        WindowManager.SetContentView(value.ModelType,true,null);
    }

    public ObservableCollection<ListItemTemplate> MenuItems { get; } = new();

    public MainWindowViewModel()
    {
        WindowManager.MainWindow = this;
        MenuItems.Add(new ListItemTemplate("Uživatelé", typeof(UzivateleViewModel)));
        MenuItems.Add(new ListItemTemplate("Jízdy", typeof(JizdyViewModel)));
        MenuItems.Add(new ListItemTemplate("Zastávky", typeof(ZastavkyViewModel)));
        
        
        SelectedListItem = MenuItems[0];
        
        WindowManager.SetMenuView(ViewType.AnonymousMenu);
        CurrentMenu = WindowManager.CurrentMenuViewModel;
    }
}

