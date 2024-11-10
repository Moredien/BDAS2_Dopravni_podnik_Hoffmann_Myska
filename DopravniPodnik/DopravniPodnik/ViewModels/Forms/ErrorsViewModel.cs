using System.Collections;
using System.ComponentModel;

namespace DopravniPodnik.ViewModels.Forms;

public class ErrorsViewModel : INotifyDataErrorInfo
{
    private Dictionary<string, List<string>> _propertyErrors = new();
    public bool HasErrors => _propertyErrors.Any();
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;



    
    public IEnumerable GetErrors(string? propertyName)
    {
        return _propertyErrors.GetValueOrDefault(propertyName, null);
    }
    
    public void AddError(string propertyName, string errorMessage)
    {
        if (!_propertyErrors.ContainsKey(propertyName))
            _propertyErrors.Add(propertyName,new List<string>());
        _propertyErrors[propertyName].Add(errorMessage);
        OnErrorsChanged(propertyName);
    }

    public void OnErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this,new DataErrorsChangedEventArgs(propertyName));
    }

    public void ClearErrors(string propertyName)
    {
        if(_propertyErrors.Remove(propertyName))
            OnErrorsChanged(propertyName);
    }
}