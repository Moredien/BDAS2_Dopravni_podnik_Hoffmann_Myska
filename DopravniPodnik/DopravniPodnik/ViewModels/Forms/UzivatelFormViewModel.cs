using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.service;

namespace DopravniPodnik.ViewModels.Forms;

public partial class UzivatelFormViewModel : ViewModelBase
{
    [ObservableProperty] private object editedUzivatel;

    private UzivatelDTO originalUzivatel;
    private ObservableCollection<object> uzivatele;

    public UzivatelFormViewModel(ObservableCollection<object> uzivatele)
    {
        this.uzivatele = uzivatele;
        editedUzivatel = new UzivatelDTO();
    }

    public UzivatelFormViewModel(object selectedUzivatel, ObservableCollection<object> uzivatele)
    {
        originalUzivatel = (UzivatelDTO)selectedUzivatel;
        this.uzivatele = uzivatele;
        editedUzivatel =CopyUtilities.DeepClone((UzivatelDTO)selectedUzivatel);
    }

    [RelayCommand]
    void SaveChanges()
    {
        if(originalUzivatel == null)
            uzivatele.Add(editedUzivatel);
        else
            uzivatele[uzivatele.IndexOf(originalUzivatel)] = editedUzivatel;
        Exit();
    }
}