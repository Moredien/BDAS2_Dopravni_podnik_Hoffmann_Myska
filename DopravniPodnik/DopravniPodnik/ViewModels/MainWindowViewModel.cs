using System.Collections.ObjectModel;
using System.Windows.Automation.Peers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DopravniPodnik.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] 
    private ViewModelBase _currentPage = new JizdyViewModel();

    [ObservableProperty] 
    private ViewModelBase _currentMenu;
    
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
        CreateNewContentView(typeof(JizdyViewModel),ViewType.Jizdy, "Jízdy", true);
        CreateNewContentView(typeof(ZastavkyViewModel),ViewType.Zastavky, "Zastávky", true);
        CreateNewContentView(typeof(LoginViewModel),ViewType.Login, "Login", false);
        CreateNewContentView(typeof(RegisterViewModel),ViewType.Register, "Register", false);

        SelectedListItem = MenuItems[0];
        
        WindowManager.SetMenuView(ViewType.AnonymousMenu);
        CurrentMenu = WindowManager.CurrentMenuViewModel;
        Console.WriteLine();
    }

    private void CreateNewContentView(Type type, ViewType viewType, string name, bool visibleInMenu)
    {
        //excludes views that are not reachable from the side menu
        if(visibleInMenu)
            MenuItems.Add(new ListItemTemplate(name,type,viewType));
        var newViewModel = (ViewModelBase)Activator.CreateInstance(type);
        WindowManager.AddNewContentView(newViewModel, viewType); 
    }

    [RelayCommand]
    private void ChangeViewTo(ViewType viewType)
    {
        WindowManager.SetContentView(viewType);
    }
}

