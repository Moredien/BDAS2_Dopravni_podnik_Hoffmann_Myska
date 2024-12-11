using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using Oracle.ManagedDataAccess.Client;

namespace DopravniPodnik.ViewModels;

public partial class EmployeeHierarchyViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService = new();
    [ObservableProperty] private ObservableCollection<EmployeeHierarchyDTO> _items;
    
    public EmployeeHierarchyViewModel()
    {
        Items = new();
        
        FetchData();    
    }

    private async void FetchData()
    {
        string query = @"BEGIN :result := GET_EMPLOYEE_HIERARCHY(); END;";

        var parameters = new List<OracleParameter>
        {
            new OracleParameter
            {
                ParameterName = "result",
                OracleDbType = OracleDbType.RefCursor,
                Direction = ParameterDirection.Output
            }
        };
        var procedureCallWrapper = new ProcedureCallWrapper(query, parameters);
        try
        {

            var data = await Task.Run(() =>
                _databaseService.FetchDataParam<EmployeeHierarchyDTO>(procedureCallWrapper));

            foreach (var entry in data)
            {
                TrimPrefix(entry);
                Items.Add(entry);
            }
        }catch (Exception ex)
        {
            MessageBox.Show($"Error while loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void TrimPrefix(EmployeeHierarchyDTO employee)
    {
        if (!String.IsNullOrEmpty(employee.Hierarchy))
        {
            employee.Hierarchy = employee.Hierarchy.Substring(4);
        }
    }
    public override void Update()
    {
        base.Update();
        WindowManager.ReturnToSelectedContentView();
    }
}