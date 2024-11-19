using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;

namespace DopravniPodnik.ViewModels.Forms;

public partial class TypyUzivateleFormViewModel : ViewModelBase , INotifyDataErrorInfo
{
    private readonly ErrorsViewModel _errorsViewModel;
    public bool HasErrors => _errorsViewModel.HasErrors;
    public bool CanCreate => !HasErrors;

    private string? _nazev ;

    public string? Nazev
    {
        get { return editedItem.Nazev;}
        set
        {
            editedItem.Nazev = value;
            ValidateInput(nameof(Nazev));
        }
    }

    private ObservableCollection<object> collection;
    private TypyUzivatele originalItem;
    private TypyUzivatele editedItem;

    
    public TypyUzivateleFormViewModel(ObservableCollection<object> typyUzivatele)
    {
        collection = typyUzivatele;
        editedItem = new TypyUzivatele();
        _errorsViewModel = new ErrorsViewModel();
        _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
        // Nazev = "";
    }
    public TypyUzivateleFormViewModel(object selectedItem,ObservableCollection<object> typyUzivatele)
    {
        collection = typyUzivatele;
        originalItem = (TypyUzivatele)selectedItem;
        editedItem = CopyUtilities.DeepClone((TypyUzivatele)selectedItem);
        _errorsViewModel = new ErrorsViewModel();
        _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
        // Nazev = editedItem.Nazev;
    }

    private void ErrorsViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        ErrorsChanged?.Invoke(this,e);
        OnPropertyChanged(nameof(CanCreate));
    }


    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;


    [RelayCommand]
    private void Submit()
    {
        ValidateAllInputs();
        
        if (CanCreate)
        {
            // save or add here
            if(originalItem == null)
                collection.Add(editedItem);
            else
                collection[collection.IndexOf(originalItem)] = editedItem;
            Exit();
        }
  
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
            case nameof(Nazev):
                if(Nazev ==null || Nazev.Length == 0)
                    _errorsViewModel.AddError(nameof(Nazev),"Název nesmí být prázdný.");
                else if (Nazev.Length > 30) 
                    _errorsViewModel.AddError(nameof(Nazev),"Neplatný název. Maximální délka je 30 znaků.");
                OnPropertyChanged(nameof(Nazev));
                break;
        }
    }

    private void ValidateAllInputs()
    {
        ValidateInput(nameof(Nazev));
    }
}