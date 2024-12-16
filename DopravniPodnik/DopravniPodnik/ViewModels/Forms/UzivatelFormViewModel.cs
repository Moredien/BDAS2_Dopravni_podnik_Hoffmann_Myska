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
    
    [ObservableProperty] private string _uzivatelskeJmeno = "";
    [ObservableProperty] private string _jmeno = "";
    [ObservableProperty] private string _prijmeni = "";
    [ObservableProperty] private DateTime _datumNarozeni = DateTime.Today;
    [ObservableProperty] private string _mesto = "";
    [ObservableProperty] private string _ulice = "";
    [ObservableProperty] private string _cisloPopisne = "";
    [ObservableProperty] private string _nazevTypUzivatele = "";

    private UzivatelDTO? _editedItem;


    public UzivatelFormViewModel(object? selectedItem)
    {
        _errorsViewModel = new ErrorsViewModel();
        _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
        if (selectedItem != null)
        {
            var item = (UzivatelDTO)selectedItem;
            UzivatelskeJmeno = item.uzivatelske_jmeno;
            Jmeno = item.jmeno;
            Prijmeni = item.prijmeni;
            DatumNarozeni = item.datum_narozeni;
            Mesto = item.mesto;
            Ulice = item.ulice;
            CisloPopisne = item.cislo_popisne.ToString();
            NazevTypUzivatele = item.nazev_typ_uzivatele;
            
            _editedItem = item;

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
                    :p_cislo_popisne
                );
            END;
        ";
        
            var parameters = new List<OracleParameter>
            {
                new OracleParameter("p_id_uzivatele", OracleDbType.Decimal)
                    { Value = _editedItem.id_uzivatele, Direction = ParameterDirection.InputOutput },
                new OracleParameter("p_uzivatelske_jmeno", OracleDbType.Varchar2)
                    { Value = UzivatelskeJmeno, Direction = ParameterDirection.Input },
                new OracleParameter("p_heslo", OracleDbType.Varchar2)
                    { Value = _editedItem.heslo, Direction = ParameterDirection.Input },
                new OracleParameter("p_jmeno", OracleDbType.Varchar2)
                    { Value = Jmeno, Direction = ParameterDirection.Input },
                new OracleParameter("p_prijmeni", OracleDbType.Varchar2)
                    { Value = Prijmeni, Direction = ParameterDirection.Input },
                new OracleParameter("p_cas_zalozeni", OracleDbType.Date)
                    { Value = _editedItem.cas_zalozeni, Direction = ParameterDirection.Input },
                new OracleParameter("p_datum_narozeni", OracleDbType.Date)
                    { Value = DatumNarozeni, Direction = ParameterDirection.Input },
                new OracleParameter("p_mesto", OracleDbType.Varchar2)
                    { Value = Mesto, Direction = ParameterDirection.Input },
                new OracleParameter("p_ulice", OracleDbType.Varchar2)
                    { Value = Ulice, Direction = ParameterDirection.Input },
                new OracleParameter("p_cislo_popisne", OracleDbType.Decimal)
                    { Value = CisloPopisne, Direction = ParameterDirection.Input },
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

    [RelayCommand]
    public void ChangeUserType()
    {
        if (_editedItem == null || _editedItem.id_uzivatele == null)
            return;
        if(NazevTypUzivatele == "Zákazník")
            WindowManager.SetContentView(typeof(ZamestnanciFormViewModel), new object[] { _editedItem.id_uzivatele });
        else if (NazevTypUzivatele == "Zaměstnanec")
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
                    { Value = _editedItem.id_uzivatele, Direction = ParameterDirection.Input },
            };

            var procedureCallWrapper = new ProcedureCallWrapper(query, parameters);
            _databaseService.ExecuteDbCall(procedureCallWrapper, out var error);
            NazevTypUzivatele = "Zákazník";
        }else if (NazevTypUzivatele == "Zaměstnanec")
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
            case nameof(UzivatelskeJmeno):
                if (string.IsNullOrEmpty(UzivatelskeJmeno))
                    _errorsViewModel.AddError(nameof(UzivatelskeJmeno), "Položka nesmí být prázdná.");
                else if (UzivatelskeJmeno.Length > 30)
                    _errorsViewModel.AddError(nameof(UzivatelskeJmeno), "Maximální délka je 30 znaků.");
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
            case nameof(DatumNarozeni):
                if (DatumNarozeni > DateTime.Today)
                    _errorsViewModel.AddError(nameof(DatumNarozeni), "Datum narození nemůže být v budoucnosti.");
                else if (DatumNarozeni.Year < 1900)
                    _errorsViewModel.AddError(nameof(DatumNarozeni), "Spodní limit roku narození je 1900.");
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
            case nameof(CisloPopisne):
                if (string.IsNullOrEmpty(CisloPopisne))
                    _errorsViewModel.AddError(nameof(CisloPopisne), "Nebylo zadáno číslo popisné.");
                else if (!Int32.TryParse(CisloPopisne, out _))
                    _errorsViewModel.AddError(nameof(CisloPopisne), "Zadaná hodnota musí být číslo");
                else if (CisloPopisne.Length > 6)
                    _errorsViewModel.AddError(nameof(CisloPopisne), "Maximální délka je 6 znaků.");
                break;
        }
    }

    private void ValidateAllInputs()
    {
        ValidateInput(nameof(UzivatelskeJmeno));
        ValidateInput(nameof(Jmeno));
        ValidateInput(nameof(Prijmeni));
        ValidateInput(nameof(DatumNarozeni));
        ValidateInput(nameof(Mesto));
        ValidateInput(nameof(Ulice));
        ValidateInput(nameof(CisloPopisne));
    }
    


    private void UpdateZmenitTypUzivateleSection()
    {
        if (UserSession.Instance.UserType.IdTypUzivatele == 3)
            ZmenaTypuUzivateleBtnVisible = Visibility.Collapsed;
        else
            ZmenaTypuUzivateleBtnVisible = Visibility.Visible;
        
        if (NazevTypUzivatele == "Zákazník")
        {
            ZmenaTypuUzivateleBtnContent = "Vytvořit zaměstnance";
        }
        else if (NazevTypUzivatele == "Zaměstnanec")
        {
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
        if (propertyChangedEventArgs.PropertyName != null) ValidateInput(propertyChangedEventArgs.PropertyName);
    }
}