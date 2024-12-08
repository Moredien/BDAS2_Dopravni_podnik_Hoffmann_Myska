using System.Collections;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;
using System.Windows;
using System.Windows.Documents;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using DopravniPodnik.ViewModels.Menu;
using DopravniPodnik.Views.Menu;
using Oracle.ManagedDataAccess.Client;

namespace DopravniPodnik.ViewModels;

public partial class GenericGridViewModel : ViewModelBase
{
    [ObservableProperty] private ObservableCollection<object> _items;

    public ObservableCollection<DataGridColumnInfo> Columns => _dataContext.Columns;
    private Type EditFormtype => _dataContext.EditFormType;

    [ObservableProperty] private object? _selectedItem;

    private readonly DataGridDataContext _dataContext;
    private readonly Type _modelType;
    private readonly string _tableName;

    private readonly DatabaseService _databaseService = new();

    partial void OnSelectedItemChanged(object? value)
    {
        // hide/show emulate button when list of users is shown
        if (WindowManager.CurrentMenuViewModel?.GetType() == typeof(AdminMenuViewModel))
        {
            var menu = (AdminMenuViewModel)WindowManager.CurrentMenuViewModel;
            if (value != null)
                menu.EmulateBtnVisibility = Visibility.Visible;
            else
                menu.EmulateBtnVisibility = Visibility.Hidden;
        }
    }

    public GenericGridViewModel(Type modelType)
    {
        _modelType = modelType;
        Items = new();

        _dataContext = GridViewTemplates.Get(modelType);

        _tableName = TableMapper.getTableName(modelType);

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

        WindowManager.SetContentView(EditFormtype, new object[] { SelectedItem });
    }

    [RelayCommand]
    void Create()
    {
        WindowManager.SetContentView(EditFormtype, new object[] { null });
    }

    [RelayCommand]
    void Delete()
    {
        if (SelectedItem != null)
        {
            var id = GetId(SelectedItem);
            var columnName = GetColumnNameForIdProperty(SelectedItem);

            string query = $"DELETE FROM {_tableName} WHERE {columnName} = {id}";

            var procedureCallWrapper = new ProcedureCallWrapper(query, new());
            _databaseService.ExecuteDbCall(procedureCallWrapper, out var error);

            Items.Remove(SelectedItem);
        }
    }

    [RelayCommand]
    void Reload()
    {
        Items.Clear();
        var method = typeof(DatabaseService).GetMethod(nameof(_databaseService.FetchData));
        var genericMethod = method?.MakeGenericMethod(_modelType);
        var data = genericMethod?.Invoke(_databaseService, new object?[] { $"SELECT * FROM ST67028.{_tableName}" });
        if (data == null)
            return;
        foreach (var obj in (IEnumerable)data)
        {
            Items.Add(obj);
        }
    }
    private int? GetId(object obj)
    {
        var type = obj.GetType();
        var idProperty = type.GetProperties()
            .FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(IdProperty)));

        return idProperty?.GetValue(obj) as int?;
    }

    public static string? GetColumnNameForIdProperty(object obj)
    {
        var properties = obj.GetType().GetProperties();

        foreach (var property in properties)
        {
            // Check if the property has both attributes
            var hasIdProperty = Attribute.IsDefined(property, typeof(IdProperty));
            var columnNameAttribute = property.GetCustomAttribute<ColumnNameAttribute>();

            if (hasIdProperty && columnNameAttribute != null)
            {
                return columnNameAttribute.Name; // Return the column name
            }
        }

        return null; // Return null if no matching property is found
    }

    public override void Update()
    {
        WindowManager.ReturnToSelectedContentView();
    }
}