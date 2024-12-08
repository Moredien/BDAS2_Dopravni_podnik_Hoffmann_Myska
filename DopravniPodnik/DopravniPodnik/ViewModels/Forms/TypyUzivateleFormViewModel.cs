using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls.Primitives;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using Oracle.ManagedDataAccess.Client;

namespace DopravniPodnik.ViewModels.Forms;

public partial class TypyUzivateleFormViewModel : ViewModelBase , INotifyDataErrorInfo
{
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    private readonly ErrorsViewModel _errorsViewModel;
    public bool HasErrors => _errorsViewModel.HasErrors;
    public bool CanCreate => !HasErrors;
    private readonly DatabaseService _databaseService = new();

    [ObservableProperty]
    private string? _nazev ="";
    private int? IdTypUzivatele { get; set; }
    public TypyUzivateleFormViewModel(object? selectedItem)
    {
        _errorsViewModel = new ErrorsViewModel();
        _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
        if (selectedItem != null)
        {
            Nazev = ((TypyUzivatele)selectedItem).Nazev;
            IdTypUzivatele = ((TypyUzivatele)selectedItem).IdTypUzivatele;
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
                ST67028.INSERT_UPDATE.edit_typy_uzivatele(
                    :p_id_typ_uzivatele,
                    :p_nazev
                );
            END;
        ";
            
            object id;
            if (IdTypUzivatele == null)
                id = DBNull.Value;
            else
                id = IdTypUzivatele;
            var parameters = new List<OracleParameter>
            {
                new OracleParameter("p_id_typ_uzivatele", OracleDbType.Decimal)
                    { Value = id, Direction = ParameterDirection.Input },
                new OracleParameter("p_nazev", OracleDbType.Varchar2) 
                    { Value = Nazev, Direction = ParameterDirection.Input }
            };
            var procedureCallWrapper = new ProcedureCallWrapper(query, parameters);
            _databaseService.ExecuteDbCall(procedureCallWrapper, out var error);

            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show($"Při ukládání data do databáze došlo k chybě", "Chyba pri ukladani", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            Exit();
        }
    }

    private void ValidateInput(string propertyName)
    {
        _errorsViewModel.ClearErrors(propertyName);
        
        switch (propertyName)
        {
            case nameof(Nazev):
                if(Nazev ==null || Nazev.Length == 0)
                    _errorsViewModel.AddError(nameof(Nazev),"Název nesmí být prázdný.");
                else if (Nazev.Length > 30) 
                    _errorsViewModel.AddError(nameof(Nazev),"Neplatný název. Maximální délka je 30 znaků.");
                break;
        }
    }

    private void ValidateAllInputs()
    {
        ValidateInput(nameof(Nazev));
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
        if (propertyChangedEventArgs.PropertyName != null) ValidateInput(propertyChangedEventArgs.PropertyName);
    }
}