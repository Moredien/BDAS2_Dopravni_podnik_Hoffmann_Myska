using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;

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
    
    private readonly DatabaseService _databaseService = new();

    public GenericGridViewModel(UserService userService, Type modelType)
    {
        _userService = userService;
        _modelType = modelType;

        DataContext = GridViewTemplates.Get(modelType);

        string tableName = TableMapper.getTableName(modelType);

        var method = typeof(DatabaseService).GetMethod(nameof(_databaseService.FetchData));
        var genericMethod = method.MakeGenericMethod(modelType);
        var data = genericMethod.Invoke(_databaseService, new object?[]{$"SELECT * FROM ST67028.{tableName}"});

        Items = new();
        foreach (var obj in (IEnumerable)data)
        {
            Items.Add(obj);
        }
            
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