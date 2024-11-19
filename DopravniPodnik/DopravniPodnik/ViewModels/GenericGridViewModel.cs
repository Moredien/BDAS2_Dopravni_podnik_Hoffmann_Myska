using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.service;

namespace DopravniPodnik.ViewModels;

public partial class GenericGridViewModel : ViewModelBase
{
    public ObservableCollection<object> Items { get; set; }

    public ObservableCollection<DataGridColumnInfo> Columns => DataContext.Columns;
    public Type EditFormtype => DataContext.EditFormType;

    [ObservableProperty] 
    public object selectedItem;

    public DataGridDataContext DataContext;
    private readonly UserService _userService;
    private readonly Type _modelType;
    
    public GenericGridViewModel(UserService userService, Type modelType)
    {
        _userService = userService;
        _modelType = modelType;

        DataContext = GridViewTemplates.Get(modelType);

        var data =  _userService.Fetch(modelType);
        Items = new();
        foreach (var obj in data)
            Items.Add(obj);
    }
    [RelayCommand]
    void Edit()
    {
        if (SelectedItem == null)
        {
            Console.WriteLine("Neni vybran zadny uzivatel");
            return;
        }
        WindowManager.SetContentView(EditFormtype,false,null,new object[]{selectedItem,Items});
    }
    [RelayCommand]
    void Create()
    {
        WindowManager.SetContentView(EditFormtype,false,null,new object[]{Items});
    }
    [RelayCommand]
    void Delete()
    {
        if(SelectedItem!=null)
            Items.Remove(SelectedItem);
    }


}