﻿using System.Collections;
using System.ComponentModel;
using System.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using Oracle.ManagedDataAccess.Client;

namespace DopravniPodnik.ViewModels.Forms;

public partial class ZastavkyFormViewModel : ViewModelBase , INotifyDataErrorInfo
{
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    private readonly ErrorsViewModel _errorsViewModel;
    public bool HasErrors => _errorsViewModel.HasErrors;
    public bool CanCreate => !HasErrors;
    private readonly DatabaseService _databaseService = new();
    
    // add all the properties here
    [ObservableProperty]
    private string? jmeno;
    
    private int? Id;

    public ZastavkyFormViewModel(object selectedItem)
    {
        _errorsViewModel = new ErrorsViewModel();
        _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
        if (selectedItem != null)
        {
            // cast to edited model type
            Jmeno = ((Zastavky)selectedItem).Jmeno;
            Id = ((Zastavky)selectedItem).IdZastavky;
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
                ST67028.INSERT_UPDATE.edit_zastavky(
                    :p_id_zastavky,
                    :p_jmeno
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
                new OracleParameter("p_id_zastavky", OracleDbType.Decimal)
                    { Value = id, Direction = ParameterDirection.Input },
                new OracleParameter("p_jmeno", OracleDbType.Varchar2) 
                    { Value = Jmeno, Direction = ParameterDirection.Input }
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