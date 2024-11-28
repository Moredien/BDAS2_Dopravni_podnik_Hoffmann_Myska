using System.Collections;
using System.ComponentModel;
using System.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using Oracle.ManagedDataAccess.Client;

namespace DopravniPodnik.ViewModels.Forms;

public partial class RidiciFormViewModel : ViewModelBase , INotifyDataErrorInfo
{
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    private readonly ErrorsViewModel _errorsViewModel;
    public bool HasErrors => _errorsViewModel.HasErrors;
    public bool CanCreate => !HasErrors;
    private readonly DatabaseService _databaseService = new();

    [ObservableProperty]
    private string? jmeno;
    [ObservableProperty]
    private string? prijmeni;

    private int? Id;

    public RidiciFormViewModel(object selectedItem)
    {
        _errorsViewModel = new ErrorsViewModel();
        _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
        if (selectedItem != null)
        {
            // cast to edited model type
            Jmeno = ((Ridici)selectedItem).Jmeno;
            Prijmeni = ((Ridici)selectedItem).Prijmeni;
            Id = ((Ridici)selectedItem).IdRidice;
        }
    }

    [RelayCommand]
    private void Submit()
    {
        ValidateAllInputs();
        
        if (CanCreate)
        {
            string query = @"
            BEGIN
                ST67028.INSERT_UPDATE.edit_ridici(
                    :p_id_ridice,
                    :p_jmeno,
                    :p_prijmeni
                );
            END;
        ";
            
            object id;
            if (Id == null)
                id = DBNull.Value;
            else
                id = Id;
            var parameters = new List<OracleParameter>
            {
                new OracleParameter("p_id_ridice", OracleDbType.Decimal)
                    { Value = id, Direction = ParameterDirection.Input },
                new OracleParameter("p_jmeno", OracleDbType.Varchar2)
                    { Value = Jmeno, Direction = ParameterDirection.Input },
                new OracleParameter("p_prijmeni", OracleDbType.Varchar2) 
                    { Value = Prijmeni, Direction = ParameterDirection.Input }
            };
            var procedureCallWrapper = new ProcedureCallWrapper(query, parameters);
            _databaseService.ExecuteDbCall(procedureCallWrapper, out var error);

            Console.WriteLine(error);
            Exit();
        }
    }
    private void ValidateInput(string propertyName)
    {
        _errorsViewModel.ClearErrors(propertyName);
        
        switch (propertyName)
        {
            // case nameof(Nazev):
            //     if(Nazev ==null || Nazev.Length == 0)
            //         _errorsViewModel.AddError(nameof(Nazev),"Název nesmí být prázdný.");
            //     else if (Nazev.Length > 30) 
            //         _errorsViewModel.AddError(nameof(Nazev),"Neplatný název. Maximální délka je 30 znaků.");
            //     break;
        }
    }
    private void ValidateAllInputs()
    {
        // List all properties to be validated
        // ValidateInput(nameof(Property));
    }

    public IEnumerable GetErrors(string? propertyName)
    {
        return _errorsViewModel.GetErrors(propertyName);
    }

    private void ErrorsViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        ErrorsChanged?.Invoke(this,e);
        OnPropertyChanged(nameof(CanCreate));
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
    {
        ValidateInput(propertyChangedEventArgs.PropertyName);
    }
    }