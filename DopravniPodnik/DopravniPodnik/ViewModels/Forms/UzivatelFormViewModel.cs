using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using Oracle.ManagedDataAccess.Client;
using System.Windows.Controls;


namespace DopravniPodnik.ViewModels.Forms;

public partial class UzivatelFormViewModel : ViewModelBase, INotifyDataErrorInfo
{
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    private readonly ErrorsViewModel _errorsViewModel;

    public bool HasErrors => _errorsViewModel.HasErrors;

    public bool CanCreate => !HasErrors;
    private readonly DatabaseService _databaseService = new();

    [ObservableProperty] private string _zmenaTypuUzivateleBtnContent = "Změna typu";

    [ObservableProperty] private Visibility _zmenaTypuUzivateleBtnVisible = Visibility.Hidden;
    
    [ObservableProperty] private string uzivatelske_jmeno;
    [ObservableProperty] private string jmeno;
    [ObservableProperty] private string prijmeni;
    [ObservableProperty] private DateTime datum_narozeni = DateTime.Today;
    [ObservableProperty] private string mesto;
    [ObservableProperty] private string ulice;
    [ObservableProperty] private string cislo_popisne;
    [ObservableProperty] public string nazev_typ_uzivatele;

    private UzivatelDTO editedItem;


    public UzivatelFormViewModel(object selectedItem)
    {
        _errorsViewModel = new ErrorsViewModel();
        _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
        if (selectedItem != null)
        {
            var item = (UzivatelDTO)selectedItem;
            Uzivatelske_jmeno = item.uzivatelske_jmeno;
            Jmeno = item.jmeno;
            Prijmeni = item.prijmeni;
            Datum_narozeni = item.datum_narozeni;
            Mesto = item.mesto;
            Ulice = item.ulice;
            Cislo_popisne = item.cislo_popisne.ToString();
            Nazev_typ_uzivatele = item.nazev_typ_uzivatele;
            
            editedItem = item;

            UpdateZmenitTypUzivateleSection();
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
                ST67028.INSERT_UPDATE.EDIT_UZIVATEL_VIEW(
                    :p_id_uzivatele,
                    :p_uzivatelske_jmeno,
                    :p_heslo,
                    :p_jmeno,
                    :p_prijmeni,
                    :p_cas_zalozeni,
                    :p_datum_narozeni,
                    :p_mesto,
                    :p_ulice,
                    :p_cislo_popisne,
                    :p_foto_jmeno_souboru,
                    :p_foto_data,
                    :p_foto_datum_pridani
                );
            END;
        ";
        
            var parameters = new List<OracleParameter>
            {
                new OracleParameter("p_id_uzivatele", OracleDbType.Decimal)
                    { Value = editedItem.id_uzivatele, Direction = ParameterDirection.InputOutput },
                new OracleParameter("p_uzivatelske_jmeno", OracleDbType.Varchar2)
                    { Value = Uzivatelske_jmeno, Direction = ParameterDirection.Input },
                new OracleParameter("p_heslo", OracleDbType.Varchar2)
                    { Value = editedItem.heslo, Direction = ParameterDirection.Input },
                new OracleParameter("p_jmeno", OracleDbType.Varchar2)
                    { Value = Jmeno, Direction = ParameterDirection.Input },
                new OracleParameter("p_prijmeni", OracleDbType.Varchar2)
                    { Value = Prijmeni, Direction = ParameterDirection.Input },
                new OracleParameter("p_cas_zalozeni", OracleDbType.Date)
                    { Value = editedItem.cas_zalozeni, Direction = ParameterDirection.Input },
                new OracleParameter("p_datum_narozeni", OracleDbType.Date)
                    { Value = Datum_narozeni, Direction = ParameterDirection.Input },
                new OracleParameter("p_mesto", OracleDbType.Varchar2)
                    { Value = Mesto, Direction = ParameterDirection.Input },
                new OracleParameter("p_ulice", OracleDbType.Varchar2)
                    { Value = Ulice, Direction = ParameterDirection.Input },
                new OracleParameter("p_cislo_popisne", OracleDbType.Decimal)
                    { Value = Cislo_popisne, Direction = ParameterDirection.Input },
                new("p_foto_jmeno_souboru", OracleDbType.Varchar2, 
                    string.IsNullOrEmpty(editedItem.foto_jmeno_souboru) ? DBNull.Value : editedItem.foto_jmeno_souboru, 
                    ParameterDirection.Input),
                // new("p_foto_data", OracleDbType.Blob, 
                //     foto_data != null ? ImageToBlob(foto_data) : DBNull.Value, 
                //     ParameterDirection.Input),                
                new("p_foto_data", OracleDbType.Blob)
                    {Value = editedItem.foto_data, Direction = ParameterDirection.Input},
                new("p_foto_datum_pridani", OracleDbType.Date, 
                    editedItem.foto_datum_pridani != default ? editedItem.foto_datum_pridani : DBNull.Value, 
                    ParameterDirection.Input)
            };

            var procedureCallWrapper = new ProcedureCallWrapper(query, parameters);
            _databaseService.ExecuteDbCall(procedureCallWrapper, out var error);

            Console.WriteLine(error);
            Exit();
        }
    }

    [RelayCommand]
    public void ChangeUserType()
    {
        if(nazev_typ_uzivatele == "Zákazník")
            WindowManager.SetContentView(typeof(ZamestnanciFormViewModel), new object[] { editedItem.id_uzivatele });
        else if (nazev_typ_uzivatele == "Zaměstnanec")
        {
            string query = @"
            BEGIN
                DELETE FROM ZAMESTNANCI WHERE ID_UZIVATELE = :id_uzivatele;
                commit;
            END;
        ";
        
            var parameters = new List<OracleParameter>
            {
                new OracleParameter("id_uzivatele", OracleDbType.Decimal)
                    { Value = editedItem.id_uzivatele, Direction = ParameterDirection.Input },
            };

            var procedureCallWrapper = new ProcedureCallWrapper(query, parameters);
            _databaseService.ExecuteDbCall(procedureCallWrapper, out var error);
            Nazev_typ_uzivatele = "Zákazník";
        }else if (nazev_typ_uzivatele == "Zaměstnanec")
        {
            //TODO remove employee?? 
            Console.WriteLine("Not implemented yet");
        }
    }

    private void ValidateInput(string propertyName)
    {
        _errorsViewModel.ClearErrors(propertyName);

        switch (propertyName)
        {
            case nameof(Uzivatelske_jmeno):
                if (string.IsNullOrEmpty(Uzivatelske_jmeno))
                    _errorsViewModel.AddError(nameof(Uzivatelske_jmeno), "Položka nesmí být prázdná.");
                else if (Uzivatelske_jmeno.Length > 30)
                    _errorsViewModel.AddError(nameof(Uzivatelske_jmeno), "Maximální délka je 30 znaků.");
                break;
            case nameof(Jmeno):
                if (string.IsNullOrEmpty(Jmeno))
                    _errorsViewModel.AddError(nameof(Jmeno), "Položka nesmí být prázdná.");
                else if (Jmeno.Length > 20)
                    _errorsViewModel.AddError(nameof(Jmeno), "Maximální délka je 20 znaků.");
                break;
            case nameof(Prijmeni):
                if (string.IsNullOrEmpty(Prijmeni))
                    _errorsViewModel.AddError(nameof(Prijmeni), "Položka nesmí být prázdná.");
                else if (Prijmeni.Length > 20)
                    _errorsViewModel.AddError(nameof(Prijmeni), "Maximální délka je 20 znaků.");
                break;
            case nameof(Datum_narozeni):
                if (Datum_narozeni > DateTime.Today)
                    _errorsViewModel.AddError(nameof(Datum_narozeni), "Datum narození nemůže být v budoucnosti.");
                else if (Datum_narozeni.Year < 1900)
                    _errorsViewModel.AddError(nameof(Datum_narozeni), "Spodní limit roku narození je 1900.");
                break;
            case nameof(Mesto):
                if (string.IsNullOrEmpty(Mesto))
                    _errorsViewModel.AddError(nameof(Mesto), "Položka nesmí být prázdná.");
                else if (Mesto.Length > 32)
                    _errorsViewModel.AddError(nameof(Mesto), "Maximální délka je 32 znaků.");
                break;
            case nameof(Ulice):
                if (string.IsNullOrEmpty(Ulice))
                    _errorsViewModel.AddError(nameof(Ulice), "Položka nesmí být prázdná.");
                else if (Ulice.Length > 32)
                    _errorsViewModel.AddError(nameof(Ulice), "Maximální délka je 32 znaků.");
                break;
            case nameof(Cislo_popisne):
                if (string.IsNullOrEmpty(Cislo_popisne))
                    _errorsViewModel.AddError(nameof(Cislo_popisne), "Nebylo zadáno číslo popisné.");
                else if (!Int32.TryParse(Cislo_popisne, out _))
                    _errorsViewModel.AddError(nameof(Cislo_popisne), "Zadaná hodnota musí být číslo");
                else if (Cislo_popisne.Length > 6)
                    _errorsViewModel.AddError(nameof(Cislo_popisne), "Maximální délka je 6 znaků.");
                break;
        }
    }

    private void ValidateAllInputs()
    {
        ValidateInput(nameof(Uzivatelske_jmeno));
        ValidateInput(nameof(Jmeno));
        ValidateInput(nameof(Prijmeni));
        ValidateInput(nameof(Datum_narozeni));
        ValidateInput(nameof(Mesto));
        ValidateInput(nameof(Ulice));
        ValidateInput(nameof(Cislo_popisne));
    }
    


    private void UpdateZmenitTypUzivateleSection()
    {
        if (Nazev_typ_uzivatele == "Zákazník")
        {
            ZmenaTypuUzivateleBtnVisible = Visibility.Visible;
            ZmenaTypuUzivateleBtnContent = "Vytvořit zaměstnance";
        }
        else if (Nazev_typ_uzivatele == "Zaměstnanec")
        {
            ZmenaTypuUzivateleBtnVisible = Visibility.Visible;
            ZmenaTypuUzivateleBtnContent = "Odebrat zaměstnance";
        }
    }

    public IEnumerable GetErrors(string? propertyName)
    {
        return _errorsViewModel.GetErrors(propertyName);
    }

    private void ErrorsViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        ErrorsChanged?.Invoke(this, e);
        OnPropertyChanged(nameof(CanCreate));
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
    {
        ValidateInput(propertyChangedEventArgs.PropertyName);
    }
}