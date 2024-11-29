using System.Collections;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using DopravniPodnik.ViewModels.Menu;

namespace DopravniPodnik.ViewModels.Forms;

public partial class LoginViewModel : ViewModelBase , INotifyDataErrorInfo
{
    private readonly ErrorsViewModel _errorsViewModel;
    private readonly AuthService _authService = new();
    public bool HasErrors => _errorsViewModel.HasErrors;
    public bool CanCreate => !HasErrors;

    private string? _uzivatelske_jmeno ;

    public string? Uzivatelske_jmeno
    {
        get { return _uzivatelske_jmeno;}
        set
        {
            _uzivatelske_jmeno = value;
            ValidateInput(nameof(Uzivatelske_jmeno));
        }
    }

    [ObservableProperty]
    public PasswordBoxModel heslo = new();


    public LoginViewModel()
    {
        _errorsViewModel = new ErrorsViewModel();
        _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
    }

    private void ErrorsViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        ErrorsChanged?.Invoke(this,e);
        OnPropertyChanged(nameof(CanCreate));
    }


    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;


    [RelayCommand]
    private void Login()
    {
        // Console.WriteLine($"pw: {PasswordBoxHelper.ConvertToUnsecureString(Heslo)}");
        ValidateAllInputs();
        
        //do some authentication
        if (!CanCreate) return;

        _authService.LoginUser(Uzivatelske_jmeno!, PasswordBoxHelper.ConvertToUnsecureString(Heslo.Value));
        
        WindowManager.SetMenuView(typeof(LoggedInUserMenuViewModel));
        // Exit();

    }
    public IEnumerable GetErrors(string? propertyName)
    {
        return _errorsViewModel.GetErrors(propertyName);
    }


    private void ValidateInput(string propertyName)
    {
        _errorsViewModel.ClearErrors(propertyName);
        
        switch (propertyName)
        {
            case nameof(Heslo):
                if(Heslo.Validate() is {} value)
                    _errorsViewModel.AddError(nameof(Heslo),value);
                break;
            case nameof(Uzivatelske_jmeno):
                if(Uzivatelske_jmeno ==null || Uzivatelske_jmeno.Length == 0)
                    _errorsViewModel.AddError(nameof(Uzivatelske_jmeno),"Nebylo zadáno uživatelské jméno.");
                else if (_uzivatelske_jmeno.Length > 30) 
                    _errorsViewModel.AddError(nameof(Uzivatelske_jmeno),"Neplatné uživatelské jméno. Maximální délka je 30 znaků.");
                OnPropertyChanged(nameof(Uzivatelske_jmeno));
                break;
        }
    }

    private void ValidateAllInputs()
    {
        ValidateInput(nameof(Uzivatelske_jmeno));
        ValidateInput(nameof(Heslo));
    }
}