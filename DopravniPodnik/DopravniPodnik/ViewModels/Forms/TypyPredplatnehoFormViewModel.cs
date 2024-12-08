using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using Oracle.ManagedDataAccess.Client;

namespace DopravniPodnik.ViewModels.Forms;

public partial class TypyPredplatnehoFormViewModel : ViewModelBase , INotifyDataErrorInfo
{
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    private readonly ErrorsViewModel _errorsViewModel;
    public bool HasErrors => _errorsViewModel.HasErrors;
    public bool CanCreate => !HasErrors;
    private readonly DatabaseService _databaseService = new();
    
    [ObservableProperty]
    private string? _jmeno;
    [ObservableProperty]
    private string? _cena;
    
    private int? _id;

    public TypyPredplatnehoFormViewModel(object? selectedItem)
    {
        _errorsViewModel = new ErrorsViewModel();
        _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
        if (selectedItem != null)
        {
            // cast to edited model type
            Jmeno = ((TypyPredplatneho)selectedItem).Jmeno;
            Cena = ((TypyPredplatneho)selectedItem).Cena.ToString();
            _id = ((TypyPredplatneho)selectedItem).IdTypPredplatneho;
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
                ST67028.INSERT_UPDATE.edit_typy_predplatneho(
                    :p_id_typ_predplatneho,
                    :p_jmeno,
                    :p_cena
                );
            END;
        ";
            
            object id;
            if (_id == null)
                id = DBNull.Value;
            else
                id = _id;
            var parameters = new List<OracleParameter>
            {
                new OracleParameter("p_id_typ_uzivatele", OracleDbType.Decimal)
                    { Value = id, Direction = ParameterDirection.Input },
                new OracleParameter("p_jmeno", OracleDbType.Varchar2) 
                    { Value = Jmeno, Direction = ParameterDirection.Input },
                new OracleParameter("p_cena", OracleDbType.Varchar2) 
                    { Value = Int32.Parse(Cena), Direction = ParameterDirection.Input }
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
            case nameof(Cena):
                if(Cena==null || Cena.Length==0)
                    _errorsViewModel.AddError(nameof(Cena),"Nebylo zadána cena.");
                else if(!Int32.TryParse(Cena,out _))
                    _errorsViewModel.AddError(nameof(Cena),"Zadaná hodnota musí být číslo");
                else if(Cena.Length>10)
                    _errorsViewModel.AddError(nameof(Cena),"Maximální délka je 10 znaků.");
                break;
        }
    }
    private void ValidateAllInputs()
    {
        // List all properties to be validated
        ValidateInput(nameof(Cena));
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