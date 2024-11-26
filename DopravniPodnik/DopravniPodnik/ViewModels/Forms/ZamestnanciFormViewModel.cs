using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices.JavaScript;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.Models;

namespace DopravniPodnik.ViewModels.Forms;

public partial class ZamestnanciFormViewModel: ViewModelBase , INotifyDataErrorInfo
{
    private readonly ErrorsViewModel _errorsViewModel;
    public bool HasErrors => _errorsViewModel.HasErrors;
    public bool CanCreate => !HasErrors;
    private void ErrorsViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        ErrorsChanged?.Invoke(this,e);
        OnPropertyChanged(nameof(CanCreate));
    }


    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    
    public string plat;
    public string? Plat
    {
        get { return plat;}
        set
        {
            plat = value;
            ValidateInput(nameof(Plat));
        }
    }

    public DateTime? platnostUvazkuDo;

    public DateTime? PlatnostUvazkuDo
    {
        get { return platnostUvazkuDo; }
        set
        {
            platnostUvazkuDo = value;
            ValidateInput(nameof(PlatnostUvazkuDo));
        }
    }

    public ZamestnanciFormViewModel(object selectedItem)
    {
        if (selectedItem != null)
        {
            var zamestannec = (Zamestnanci)selectedItem;
            Plat = zamestannec.Plat;
            PlatnostUvazkuDo = zamestannec.PlatnostUvazkuDo;
        }
    }

    [RelayCommand]
    public void Submit()
    {
        
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
            case nameof(Plat):
                //validate hee
                OnPropertyChanged(nameof(Plat));
                break;
            case nameof(PlatnostUvazkuDo):
                //and here
                break;
        }
    }

    private void ValidateAllInputs()
    {
        ValidateInput(nameof(Plat));
        ValidateInput(nameof(PlatnostUvazkuDo));
    }
}