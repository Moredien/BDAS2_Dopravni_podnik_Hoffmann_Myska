using System.Collections;
using System.ComponentModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;

namespace DopravniPodnik.ViewModels.Forms;

public partial class UzivatelFormViewModel : ViewModelBase , INotifyDataErrorInfo
{
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    private readonly ErrorsViewModel _errorsViewModel;

    public bool HasErrors => _errorsViewModel.HasErrors;

    public bool CanCreate => !HasErrors;

    [ObservableProperty]
    private string _zmenaTypuUzivateleBtnContent = "Změna typu";

    [ObservableProperty]
    private Visibility _zmenaTypuUzivateleBtnVisible = Visibility.Hidden;



    [ObservableProperty] 
    private Visibility _editBtnVisible = Visibility.Hidden;

    [ObservableProperty] private string uzivatelske_jmeno;
    [ObservableProperty] private string jmeno;
    [ObservableProperty] private string prijmeni;
    [ObservableProperty] private DateTime datum_narozeni = DateTime.Today;
    [ObservableProperty] private string mesto;
    [ObservableProperty] private string ulice;
    [ObservableProperty] private string cislo_popisne;
    [ObservableProperty] private string nazev_typ_uzivatele;

    private int? idZamestnance = null;
    
    
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

            idZamestnance = item.id_zamestnance;
            
            UpdateZmenitTypUzivateleSection();
        }
    }

    [RelayCommand]
    private void Submit()
    {
        ValidateAllInputs();
        
        if (CanCreate)
        {
            //insert/update procedure
            Exit();
        }
    }
    
    private void ValidateInput(string propertyName)
    {
        _errorsViewModel.ClearErrors(propertyName);
        
        switch (propertyName)
        {
            case nameof(Uzivatelske_jmeno):
                    if(Uzivatelske_jmeno.Length ==0)
                        _errorsViewModel.AddError(nameof(Uzivatelske_jmeno),"Položka nesmí být prázdná.");
                    if(Uzivatelske_jmeno.Length > 30)
                        _errorsViewModel.AddError(nameof(Uzivatelske_jmeno),"Maximální délka je 30 znaků.");
                break;
            case nameof(Jmeno):
                if(Jmeno.Length ==0)
                    _errorsViewModel.AddError(nameof(Jmeno),"Položka nesmí být prázdná.");
                if(Jmeno.Length > 20)
                    _errorsViewModel.AddError(nameof(Jmeno),"Maximální délka je 20 znaků.");
                break;
            case nameof(Prijmeni):
                if(Prijmeni.Length ==0)
                    _errorsViewModel.AddError(nameof(Prijmeni),"Položka nesmí být prázdná.");
                if(Prijmeni.Length > 20)
                    _errorsViewModel.AddError(nameof(Prijmeni),"Maximální délka je 20 znaků.");
                break;
            case nameof(Datum_narozeni):
                if(Datum_narozeni>DateTime.Today)
                    _errorsViewModel.AddError(nameof(Datum_narozeni),"Datum narození nemůže být v budoucnosti.");
                else if(Datum_narozeni.Year<1900)
                    _errorsViewModel.AddError(nameof(Datum_narozeni),"Spodní limit roku narození je 1900.");
                break;
            case nameof(Mesto):
                if(Mesto.Length ==0)
                    _errorsViewModel.AddError(nameof(Mesto),"Položka nesmí být prázdná.");
                if(Mesto.Length > 32)
                    _errorsViewModel.AddError(nameof(Mesto),"Maximální délka je 32 znaků.");
                break;
            case nameof(Ulice):
                if(Ulice.Length ==0)
                    _errorsViewModel.AddError(nameof(Ulice),"Položka nesmí být prázdná.");
                if(Ulice.Length > 32)
                    _errorsViewModel.AddError(nameof(Ulice),"Maximální délka je 32 znaků.");
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
        ValidateInput(nameof(Jmeno));
        ValidateInput(nameof(Prijmeni));
        ValidateInput(nameof(Datum_narozeni));
        ValidateInput(nameof(Mesto));
        ValidateInput(nameof(Ulice));
        ValidateInput(nameof(Cislo_popisne));
    }
    
    // [RelayCommand]
    // void ChangeUserType()
    // {
    //     if (Nazev_typ_uzivatele == "Zákazník")
    //     {
    //         // open form to create employee
    //     }
    //     else if (Nazev_typ_uzivatele == "Zaměstnanec")
    //     {
    //         // remove employee here
    //         Nazev_typ_uzivatele = "Zákazník";
    //     }
    //     UpdateZmenitTypUzivateleSection();
    // }
    
    [RelayCommand]
    private void EditEmployee()
    {
        Console.WriteLine($"editing employee {idZamestnance}");
        WindowManager.SetContentView(typeof(ZamestnanciFormViewModel),new object[]{idZamestnance});
    }

    private void UpdateZmenitTypUzivateleSection()
    {
        if (Nazev_typ_uzivatele == "Zákazník")
        {
            EditBtnVisible = Visibility.Hidden;

            ZmenaTypuUzivateleBtnVisible = Visibility.Visible;
            ZmenaTypuUzivateleBtnContent = "Vytvořit zaměstnance";
        }
        else if (Nazev_typ_uzivatele == "Zaměstnanec")
        {
            EditBtnVisible = Visibility.Visible;

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
        ErrorsChanged?.Invoke(this,e);
        OnPropertyChanged(nameof(CanCreate));
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
    {
        ValidateInput(propertyChangedEventArgs.PropertyName);
    }
}