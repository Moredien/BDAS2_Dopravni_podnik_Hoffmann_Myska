using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.Models;
using DopravniPodnik.ViewModels.Forms;

namespace DopravniPodnik.ViewModels;

public partial class RegisterViewModel : ViewModelBase , INotifyDataErrorInfo
{
    private readonly ErrorsViewModel _errorsViewModel;
    public bool HasErrors => _errorsViewModel.HasErrors;
    public bool CanCreate => !HasErrors;
    
    [ObservableProperty]
    private string uzivatelske_jmeno;
    [ObservableProperty]
    public PasswordBoxModel heslo_1 = new();    
    [ObservableProperty]
    public PasswordBoxModel heslo_2 = new();
    [ObservableProperty]
    private string jmeno;
    [ObservableProperty]
    private string prijmeni;
    [ObservableProperty]
    private DateTime datum_narozeni = DateTime.Today;
    [ObservableProperty]
    private string mesto;
    [ObservableProperty]
    private string ulice;
    [ObservableProperty] 
    private string cislo_popisne;

    
    public RegisterViewModel()
    {
        _errorsViewModel = new ErrorsViewModel();
        _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
        
    }

    [RelayCommand]
    private void ExecuteRegister()
    {
        ValidateAllInputs();
        if (CanCreate)
        {
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
            case nameof(Heslo_1):
                if(Heslo_1.Validate() is {} value)
                    _errorsViewModel.AddError(nameof(Heslo_1),value);
                break;
            case nameof(Heslo_2):
                if(Heslo_2.Validate() is {} value2)
                    _errorsViewModel.AddError(nameof(Heslo_2),value2);
                break;
            case nameof(Uzivatelske_jmeno):
                if(Uzivatelske_jmeno ==null || Uzivatelske_jmeno.Length == 0)
                    _errorsViewModel.AddError(nameof(Uzivatelske_jmeno),"Nebylo zadáno uživatelské jméno.");
                else if (Uzivatelske_jmeno.Length > 30) 
                    _errorsViewModel.AddError(nameof(Uzivatelske_jmeno),"Neplatné uživatelské jméno. Maximální délka je 30 znaků.");
                break;
            case nameof(Jmeno):
                if(Jmeno ==null || Jmeno.Length == 0)
                    _errorsViewModel.AddError(nameof(Jmeno),"Nebylo zadáno jméno.");
                else if(Jmeno.Length>20)
                    _errorsViewModel.AddError(nameof(Jmeno),"Maximální délka jména je 20 znaků.");
                break;
            case nameof(Prijmeni):
                if(Prijmeni ==null || Prijmeni.Length == 0)
                    _errorsViewModel.AddError(nameof(Prijmeni),"Nebylo zadáno příjmení.");
                else if(Prijmeni.Length>20)
                    _errorsViewModel.AddError(nameof(Prijmeni),"Maximální délka příjmení je 20 znaků.");
                break;
            case nameof(Datum_narozeni):
                if(Datum_narozeni>DateTime.Today)
                    _errorsViewModel.AddError(nameof(Datum_narozeni),"Datum narození nemůže být v budoucnosti.");
                else if(Datum_narozeni.Year<1900)
                    _errorsViewModel.AddError(nameof(Datum_narozeni),"Spodní limit roku narození je 1900.");
                break;
            case nameof(Mesto):
                if(Mesto==null || Mesto.Length==0)
                    _errorsViewModel.AddError(nameof(Mesto),"Nebylo zadáno město.");
                else if(Mesto.Length>32)
                    _errorsViewModel.AddError(nameof(Mesto),"Maximální počet znaků je 32.");
                break;
            case nameof(Ulice):
                if(Ulice==null || Ulice.Length==0)
                    _errorsViewModel.AddError(nameof(Ulice),"Nebyla zadána ulice.");
                else if(Ulice.Length>32)
                    _errorsViewModel.AddError(nameof(Ulice),"Maximální počet znaků je 32.");
                break;
            case nameof(Cislo_popisne):
                if(Cislo_popisne==null || Cislo_popisne.Length==0)
                    _errorsViewModel.AddError(nameof(Cislo_popisne),"Nebylo zadáno číslo popisné.");
                else if(!Int32.TryParse(Cislo_popisne,out _))
                    _errorsViewModel.AddError(nameof(Cislo_popisne),"Zadaná hodnota musí být číslo");
                else if(Cislo_popisne.Length>6)
                    _errorsViewModel.AddError(nameof(Cislo_popisne),"Maximální délka je 6 znaků.");
                break;
        }
    }

    private void ValidateAllInputs()
    {
        ValidateInput(nameof(Uzivatelske_jmeno));
        ValidateInput(nameof(Heslo_1));
        ValidateInput(nameof(Heslo_2));
        ValidateInput(nameof(Jmeno));
        ValidateInput(nameof(Prijmeni));
        ValidateInput(nameof(Datum_narozeni));
        ValidateInput(nameof(Mesto));
        ValidateInput(nameof(Ulice));
        ValidateInput(nameof(Cislo_popisne));
    }
    
    protected override void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
    {
        ValidateInput(propertyChangedEventArgs.PropertyName);
    }

}