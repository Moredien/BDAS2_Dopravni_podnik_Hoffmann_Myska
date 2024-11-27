using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.Models;

namespace DopravniPodnik.ViewModels.Forms;

public partial class AdresyFormViewModel : ViewModelBase , INotifyDataErrorInfo
{
    private readonly ErrorsViewModel _errorsViewModel;
    public bool HasErrors => _errorsViewModel.HasErrors;
    public bool CanCreate => !HasErrors;

    [ObservableProperty]
    public string mesto;
    [ObservableProperty] 
    public string ulice;
    [ObservableProperty] 
    public string cislo_popisne;
    
    public AdresyFormViewModel(object selectedItem)
    {
        _errorsViewModel = new ErrorsViewModel();
        _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
        
        if (selectedItem != null)
        {
            var item = (Adresy)selectedItem;
            Mesto = item.Mesto;
            Ulice = item.Ulice;
            Cislo_popisne = item.CisloPopisne.ToString();
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
                if(Mesto ==null || Mesto.Length == 0)
                    _errorsViewModel.AddError(nameof(Mesto),"Nebylo zadáno město.");
                else if (Mesto.Length > 32) 
                    _errorsViewModel.AddError(nameof(Mesto),"Maximální délka je 32 znaků.");
                break;
            case nameof(Ulice):
                if(Ulice ==null || Ulice.Length == 0)
                    _errorsViewModel.AddError(nameof(Ulice),"Nebyla zadána ulice.");
                else if (Ulice.Length > 32) 
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
        ValidateInput(nameof(Mesto));
        ValidateInput(nameof(Ulice));
        ValidateInput(nameof(Cislo_popisne));
    }
    
    protected override void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
    {
        ValidateInput(propertyChangedEventArgs.PropertyName);
    }
}