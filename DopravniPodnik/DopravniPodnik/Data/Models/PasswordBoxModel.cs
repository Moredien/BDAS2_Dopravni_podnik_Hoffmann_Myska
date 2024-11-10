using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;
using DopravniPodnik.Utils;

namespace DopravniPodnik.Data.Models;

public class PasswordBoxModel : INotifyPropertyChanged
{
    private SecureString _value;
    private string _errorMessage;

    public SecureString Value
    {
        get { return _value;}
        set
        {
            // Console.WriteLine(PasswordBoxHelper.ConvertToUnsecureString(value));
            _value = value;

            Validate();
        }
    }

    public string ErrorMessage
    {
        get { return _errorMessage; }
        set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public string Validate()
    {
        ErrorMessage = null;
        if (_value == null || _value.Length == 0)
            ErrorMessage = "Nebylo zadáno heslo.";
        return ErrorMessage;
    }
}