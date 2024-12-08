using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using DopravniPodnik.ViewModels.Forms;

namespace DopravniPodnik.ViewModels.Forms;

public partial class RegisterViewModel : ViewModelBase , INotifyDataErrorInfo
{
    private readonly ErrorsViewModel _errorsViewModel;
    private readonly AuthService _authService = new();
    private readonly Logger _logger = App.LoggerInstance;
    
    public bool HasErrors => _errorsViewModel.HasErrors;
    public bool CanCreate => !HasErrors;
    
    [ObservableProperty]
    private string _uzivatelskeJmeno = "";
    [ObservableProperty]
    public PasswordBoxModel heslo1 = new();    
    [ObservableProperty]
    public PasswordBoxModel heslo2 = new();
    [ObservableProperty]
    private string _jmeno = "";
    [ObservableProperty]
    private string _prijmeni = "";
    [ObservableProperty]
    private DateTime _datumNarozeni = DateTime.Today;
    [ObservableProperty]
    private string _mesto = "";
    [ObservableProperty]
    private string _ulice = "";
    [ObservableProperty] 
    private string _cisloPopisne = "";

    public RegisterViewModel()
    {
        _errorsViewModel = new ErrorsViewModel();
        _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
    }

    [RelayCommand]
    private void ExecuteRegister()
    {
        ValidateAllInputs();
        if (!CanCreate) return;
        
        var result = _authService.RegisterUser(new UzivatelDTO
        {
            uzivatelske_jmeno = UzivatelskeJmeno,
            heslo = PasswordBoxHelper.ConvertToUnsecureString(Heslo1.Value), 
            jmeno = Jmeno,
            prijmeni = Prijmeni,
            datum_narozeni = DatumNarozeni,
            mesto = Mesto,
            ulice = Ulice,
            cislo_popisne = !string.IsNullOrEmpty(CisloPopisne) && int.TryParse(CisloPopisne, out var cp) ? (short)cp : (short)0
        });

        // Handle the result of the registration
        switch (result)
        {
            case UserRegistrationResult.Success:
                _logger.Message("User successfully registered!").Info().Log();
                Exit();
                break;
            case UserRegistrationResult.AlreadyRegistered:
                _logger.Message($"User with username {UzivatelskeJmeno} already exists.").Warning().Log();
                break;
            case UserRegistrationResult.Failed:
                _logger.Message("User registration failed. Please try again.").Error().Log();
                break;
            default:
                throw new ArgumentOutOfRangeException();
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
            case nameof(Heslo1):
                if(Heslo1.Validate() is {} value)
                    _errorsViewModel.AddError(nameof(Heslo1),value);
                break;
            case nameof(Heslo2):
                if(Heslo2.Validate() is {} value2)
                    _errorsViewModel.AddError(nameof(Heslo2),value2);
                break;
            case nameof(UzivatelskeJmeno):
                if(string.IsNullOrEmpty(UzivatelskeJmeno))
                    _errorsViewModel.AddError(nameof(UzivatelskeJmeno),"Nebylo zadáno uživatelské jméno.");
                else if (UzivatelskeJmeno.Length > 30) 
                    _errorsViewModel.AddError(nameof(UzivatelskeJmeno),"Neplatné uživatelské jméno. Maximální délka je 30 znaků.");
                break;
            case nameof(Jmeno):
                if(string.IsNullOrEmpty(Jmeno))
                    _errorsViewModel.AddError(nameof(Jmeno),"Nebylo zadáno jméno.");
                else if(Jmeno.Length>20)
                    _errorsViewModel.AddError(nameof(Jmeno),"Maximální délka jména je 20 znaků.");
                break;
            case nameof(Prijmeni):
                if(string.IsNullOrEmpty(Prijmeni))
                    _errorsViewModel.AddError(nameof(Prijmeni),"Nebylo zadáno příjmení.");
                else if(Prijmeni.Length>20)
                    _errorsViewModel.AddError(nameof(Prijmeni),"Maximální délka příjmení je 20 znaků.");
                break;
            case nameof(DatumNarozeni):
                if(DatumNarozeni>DateTime.Today)
                    _errorsViewModel.AddError(nameof(DatumNarozeni),"Datum narození nemůže být v budoucnosti.");
                else if(DatumNarozeni.Year<1900)
                    _errorsViewModel.AddError(nameof(DatumNarozeni),"Spodní limit roku narození je 1900.");
                break;
            case nameof(Mesto):
                if(string.IsNullOrEmpty(Mesto))
                    _errorsViewModel.AddError(nameof(Mesto),"Nebylo zadáno město.");
                else if(Mesto.Length>32)
                    _errorsViewModel.AddError(nameof(Mesto),"Maximální počet znaků je 32.");
                break;
            case nameof(Ulice):
                if(string.IsNullOrEmpty(Ulice))
                    _errorsViewModel.AddError(nameof(Ulice),"Nebyla zadána ulice.");
                else if(Ulice.Length>32)
                    _errorsViewModel.AddError(nameof(Ulice),"Maximální počet znaků je 32.");
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
        ValidateInput(nameof(UzivatelskeJmeno));
        ValidateInput(nameof(Heslo1));
        ValidateInput(nameof(Heslo2));
        ValidateInput(nameof(Jmeno));
        ValidateInput(nameof(Prijmeni));
        ValidateInput(nameof(DatumNarozeni));
        ValidateInput(nameof(Mesto));
        ValidateInput(nameof(Ulice));
        ValidateInput(nameof(CisloPopisne));
    }
    
    protected override void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
    {
        ValidateInput(propertyChangedEventArgs.PropertyName);
    }

}