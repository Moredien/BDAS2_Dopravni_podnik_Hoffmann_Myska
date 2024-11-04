using System.Collections.ObjectModel;
using System.Windows.Automation.Peers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DopravniPodnik.Data.service;
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
        WindowManager.SetContentView(value.Label);
    }

    public ObservableCollection<ListItemTemplate> MenuItems { get; } = new();

    public MainWindowViewModel()
    {
        WindowManager.MainWindow = this;
        CreateNewContentView(typeof(JizdyViewModel), "Jízdy", true);
        CreateNewContentView(typeof(ZastavkyViewModel), "Zastávky", true);
        CreateNewContentView(typeof(LoginViewModel), "Login", false);
        CreateNewContentView(typeof(RegisterViewModel), "Register", false);

        SelectedListItem = MenuItems[0];
        
        WindowManager.SetMenuView("Anonymous");
        CurrentMenu = WindowManager.currentMenuViewModel;
        Console.WriteLine();
    }

    private void CreateNewContentView(Type type, string name, bool visibleInMenu)
    {
        if(visibleInMenu)
            MenuItems.Add(new ListItemTemplate(name,type));
        var newViewModel = (ViewModelBase)Activator.CreateInstance(type);
        WindowManager.AddNewContentView(newViewModel, name); 
    }

    [RelayCommand]
    private void ChangeViewTo(string name)
    {
        WindowManager.SetContentView(name);
    }
}

public class ListItemTemplate
{
    public ListItemTemplate(string label, Type modelType)
    {
        Label = label;
        ModelType = modelType;
    }

    public string Label { get; }
    public Type ModelType { get; }
}