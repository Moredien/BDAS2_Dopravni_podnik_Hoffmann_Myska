using System.Collections;
using System.ComponentModel;
using System.Windows;
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

    private string? _uzivatelskeJmeno ="";

    public string? UzivatelskeJmeno
    {
        get { return _uzivatelskeJmeno;}
        set
        {
            _uzivatelskeJmeno = value;
            ValidateInput(nameof(UzivatelskeJmeno));
        }
    }

    [ObservableProperty]
    private PasswordBoxModel _heslo = new();


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
        
        if (!CanCreate) return;

        var result = _authService.LoginUser(UzivatelskeJmeno!, PasswordBoxHelper.ConvertToUnsecureString(Heslo.Value));

        switch (result)
        {
            case UserLoginResult.Success:
                break;
            case UserLoginResult.Failed:
                MessageBox.Show("Při přihlašování se vyskytla chyba.", "Chyba přihlášení", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                break;
            case UserLoginResult.NotRegistered:
                MessageBox.Show($"Uživatel s jménem {UzivatelskeJmeno} neexistuje.", "Uzivatel neexistuje", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                break;
            case UserLoginResult.WrongPassword:
                MessageBox.Show("Špatné heslo", "Spatne heslo", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        if (UserSession.Instance.UserType == null)
            return;

        WindowManager.SetMenuView(UserSession.Instance.UserType.Nazev == "Admin"
            ? typeof(AdminMenuViewModel)
            : typeof(LoggedInUserMenuViewModel));
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
            case nameof(UzivatelskeJmeno):
                if(UzivatelskeJmeno ==null || UzivatelskeJmeno.Length == 0)
                    _errorsViewModel.AddError(nameof(UzivatelskeJmeno),"Nebylo zadáno uživatelské jméno.");
                else if (_uzivatelskeJmeno != null && _uzivatelskeJmeno.Length > 30) 
                    _errorsViewModel.AddError(nameof(UzivatelskeJmeno),"Neplatné uživatelské jméno. Maximální délka je 30 znaků.");
                OnPropertyChanged(nameof(UzivatelskeJmeno));
                break;
        }
    }

    private void ValidateAllInputs()
    {
        ValidateInput(nameof(UzivatelskeJmeno));
        ValidateInput(nameof(Heslo));
    }
}