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
    private readonly Type _modelType;
    private readonly string tableName;
    
    private readonly DatabaseService _databaseService = new();

    public GenericGridViewModel(Type modelType)
    {
        _modelType = modelType;
        Items = new();
        
        DataContext = GridViewTemplates.Get(modelType);

        tableName = TableMapper.getTableName(modelType);
        
        Reload();
    }
    [RelayCommand]
    void Edit()
    {
        if (SelectedItem == null)
        {
            Console.WriteLine("Neni vybran zadny radek");
            return;
        }
        WindowManager.SetContentView(EditFormtype,new object[]{selectedItem});
    }
    [RelayCommand]
    void Create()
    {
        WindowManager.SetContentView(EditFormtype,new object[]{null});
    }
    [RelayCommand]
    void Delete()
    {
        if(SelectedItem!=null)
            Items.Remove(SelectedItem);
    }
    [RelayCommand]
    void Reload()
    {
        Items.Clear();
        var method = typeof(DatabaseService).GetMethod(nameof(_databaseService.FetchData));
        var genericMethod = method.MakeGenericMethod(_modelType);
        var data = genericMethod.Invoke(_databaseService, new object?[]{$"SELECT * FROM ST67028.{tableName}"});

        foreach (var obj in (IEnumerable)data)
        {
            Items.Add(obj);
        }
    }


}