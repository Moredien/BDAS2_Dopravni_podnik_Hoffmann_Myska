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

public partial class AdresyFormViewModel : ViewModelBase , INotifyDataErrorInfo
{
    private readonly ErrorsViewModel _errorsViewModel;
    public bool HasErrors => _errorsViewModel.HasErrors;
    public bool CanCreate => !HasErrors;
    private readonly DatabaseService _databaseService = new();

    [ObservableProperty]
    private string _mesto= "";
    [ObservableProperty] 
    private string _ulice= "";
    [ObservableProperty] 
    private string _cisloPopisne = "";

    private int? _idAdresy; 
    
    public AdresyFormViewModel(object? selectedItem)
    {
        _errorsViewModel = new ErrorsViewModel();
        _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
        
        if (selectedItem != null)
        {
            var item = (Adresy)selectedItem;
            Mesto = item.Mesto;
            Ulice = item.Ulice;
            CisloPopisne = item.CisloPopisne.ToString();

            _idAdresy = item.IdAdresy;
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
                ST67028.INSERT_UPDATE.edit_adresy(
                    :p_id_adresy,
                    :p_mesto,
                    :p_ulice, 
                    :p_cislo_popisne
                );
            END;
        ";
            object id;
            if (_idAdresy == null)
                id = DBNull.Value;
            else
                id = _idAdresy;
        
            var parameters = new List<OracleParameter>
            {
                new OracleParameter("p_id_adresy", OracleDbType.Decimal)
                    { Value = id, Direction = ParameterDirection.Input },
                new OracleParameter("p_mesto", OracleDbType.Varchar2) 
                    { Value = Mesto, Direction = ParameterDirection.Input },
                new OracleParameter("p_ulice", OracleDbType.Varchar2)
                    { Value = Ulice, Direction = ParameterDirection.Input },
                new OracleParameter("p_cislo_popisne", OracleDbType.Decimal)
                    { Value = Int32.Parse(CisloPopisne), Direction = ParameterDirection.Input }
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

    public IEnumerable GetErrors(string? propertyName)
    {
        return _errorsViewModel.GetErrors(propertyName);
        
    }

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    private void ErrorsViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        ErrorsChanged?.Invoke(this,e);
        OnPropertyChanged(nameof(CanCreate));
    }
    private void ValidateInput(string propertyName)
    {
        _errorsViewModel.ClearErrors(propertyName);

        switch (propertyName)
        {
            case nameof(Mesto):
                if(string.IsNullOrEmpty(Mesto))
                    _errorsViewModel.AddError(nameof(Mesto),"Nebylo zadáno město.");
                else if (Mesto.Length > 32) 
                    _errorsViewModel.AddError(nameof(Mesto),"Maximální délka je 32 znaků.");
                break;
            case nameof(Ulice):
                if(string.IsNullOrEmpty(Ulice))
                    _errorsViewModel.AddError(nameof(Ulice),"Nebyla zadána ulice.");
                else if (Ulice.Length > 32) 
                    _errorsViewModel.AddError(nameof(Ulice),"Maximální délka je 32 znaků.");
                break;
            case nameof(CisloPopisne):
                if(string.IsNullOrEmpty(CisloPopisne))
                    _errorsViewModel.AddError(nameof(CisloPopisne),"Nebylo zadáno číslo popisné.");
                else if(!Int32.TryParse(CisloPopisne,out _))
                    _errorsViewModel.AddError(nameof(CisloPopisne),"Zadaná hodnota musí být číslo");
                else if(CisloPopisne.Length>6)
                    _errorsViewModel.AddError(nameof(CisloPopisne),"Maximální délka je 6 znaků.");
                break;
        }
    }
    private void ValidateAllInputs()
    {
        ValidateInput(nameof(Mesto));
        ValidateInput(nameof(Ulice));
        ValidateInput(nameof(CisloPopisne));
    }
    
    protected override void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
    {
        if (propertyChangedEventArgs.PropertyName != null) ValidateInput(propertyChangedEventArgs.PropertyName);
    }
}