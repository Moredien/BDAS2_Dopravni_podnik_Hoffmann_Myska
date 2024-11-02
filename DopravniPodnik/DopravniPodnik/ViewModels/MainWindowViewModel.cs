using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DopravniPodnik.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBase _currentPage = new JizdyViewModel();
    
    [ObservableProperty]
    private ListItemTemplate _selectedListItem;    
    partial void OnSelectedListItemChanged(ListItemTemplate? value)
    {
        if (value == null) 
            value = MenuItems[0];
        ChangeViewTo(value.ModelType);
    }
    public ObservableCollection<ListItemTemplate> MenuItems { get; } = new()
    {
        new ListItemTemplate("Jízdy",typeof(JizdyViewModel)),
        new ListItemTemplate("Zastávky" , typeof(ZastavkyViewModel))
    };

    public MainWindowViewModel()
    {
        _selectedListItem = MenuItems[0];
        var registerViewModel = new RegisterViewModel();
        registerViewModel.ExitRegistationViewAction += ChangeViewToDefault;
    }

    [RelayCommand]
    void ChangeViewTo(Type viewModelType)
    {
        if (viewModelType == null)
            viewModelType = _selectedListItem.ModelType;
        var instance = Activator.CreateInstance(viewModelType);
        if (instance == null) return;
        CurrentPage = (ViewModelBase)instance;
    }
    [RelayCommand]
    void ChangeViewToDefault()
    {
        ChangeViewTo(null);
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