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
        WindowManager.SetContentView(value.ViewTypeEnum);
    }

    public ObservableCollection<ListItemTemplate> MenuItems { get; } = new();

    public MainWindowViewModel()
    {
        WindowManager.MainWindow = this;
        CreateNewContentView(typeof(UzivateleViewModel),ViewType.Uzivatele, "Uživatelé");
        CreateNewContentView(typeof(JizdyViewModel),ViewType.Jizdy, "Jízdy");
        CreateNewContentView(typeof(ZastavkyViewModel),ViewType.Zastavky, "Zastávky");

        SelectedListItem = MenuItems[0];
        
        WindowManager.SetMenuView(ViewType.AnonymousMenu);
        CurrentMenu = WindowManager.CurrentMenuViewModel;
        Console.WriteLine();
    }

    private void CreateNewContentView(Type type, ViewType viewType, string name)
    {
        MenuItems.Add(new ListItemTemplate(name,type,viewType));
        var newViewModel = (ViewModelBase)Activator.CreateInstance(type);
        if (newViewModel == null)
            throw new Exception($"Failed to create new instance of {type}");
        WindowManager.AddNewContentView(newViewModel, viewType); 
    }

    [RelayCommand]
    private void ChangeViewTo(ViewType viewType)
    {
        WindowManager.SetContentView(viewType);
    }
}

