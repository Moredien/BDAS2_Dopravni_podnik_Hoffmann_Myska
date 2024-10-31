using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DopravniPodnik.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private string test = "test";

    [ObservableProperty] private ViewModelBase _currentPage = new JizdyViewModel();
    
    [ObservableProperty]
    private ListItemTemplate _selectedListItem;    
    partial void OnSelectedListItemChanged(ListItemTemplate? value)
    {
        if (value == null) return;
        var instance = Activator.CreateInstance(value.ModelType);
        if (instance == null) return;
        CurrentPage = (ViewModelBase)instance;
    }
    public ObservableCollection<ListItemTemplate> MenuItems { get; } = new()
    {
        new ListItemTemplate("Jízdy",typeof(JizdyViewModel)),
        new ListItemTemplate("Zastávky" , typeof(ZastavkyViewModel))
    };


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