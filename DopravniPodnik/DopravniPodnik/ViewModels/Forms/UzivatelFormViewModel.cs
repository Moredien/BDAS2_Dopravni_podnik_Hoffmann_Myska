using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.service;

namespace DopravniPodnik.ViewModels.Forms;

public partial class UzivatelFormViewModel : ViewModelBase
{
    [ObservableProperty] private UzivatelDTO editedUzivatel;

    private UzivatelDTO originalUzivatel;
    private ObservableCollection<UzivatelDTO> uzivatele;

    public UzivatelFormViewModel(ObservableCollection<UzivatelDTO> uzivatele)
    {
        this.uzivatele = uzivatele;
        editedUzivatel = new UzivatelDTO();
    }

    public UzivatelFormViewModel(UzivatelDTO selectedUzivatel, ObservableCollection<UzivatelDTO> uzivatele)
    {
        originalUzivatel = selectedUzivatel;
        this.uzivatele = uzivatele;
        editedUzivatel = CopyUtilities.DeepClone(selectedUzivatel);
    }

    [RelayCommand]
    void SaveChanges()
    {
        if(originalUzivatel == null)
            uzivatele.Add(editedUzivatel);
        else
            uzivatele[uzivatele.IndexOf(originalUzivatel)] = editedUzivatel;
        WindowManager.SetContentViewToSelected();
    }
}