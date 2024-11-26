using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;

namespace DopravniPodnik.ViewModels.Forms;

public partial class UzivatelFormViewModel : ViewModelBase
{
    [ObservableProperty] private object editedUzivatel;

    private UzivatelDTO _originalUzivatel;
    private ObservableCollection<object> uzivatele;
    [ObservableProperty]
    private string _zmenaTypuUzivateleBtnContent = "Změna typu";
    [ObservableProperty]
    private Visibility _zmenaTypuUzivateleBtnVisible = Visibility.Hidden;
    [ObservableProperty] 
    private Visibility _editBtnVisible = Visibility.Hidden;

    public UzivatelFormViewModel(ObservableCollection<object> uzivatele)
    {
        this.uzivatele = uzivatele;
        editedUzivatel = new UzivatelDTO();
    }

    public UzivatelFormViewModel(object selectedUzivatel, ObservableCollection<object> uzivatele)
    {
        _originalUzivatel = (UzivatelDTO)selectedUzivatel;
        this.uzivatele = uzivatele;
        editedUzivatel =CopyUtilities.DeepClone((UzivatelDTO)selectedUzivatel);
        
        UpdateZmenitTypUzivateleSection();
    }

    [RelayCommand]
    void SaveChanges()
    {
        if(_originalUzivatel == null)
            uzivatele.Add(EditedUzivatel);
        else
            uzivatele[uzivatele.IndexOf(_originalUzivatel)] = EditedUzivatel;
        Exit();
    }

    [RelayCommand]
    void ChangeUserType()
    {
        if (_originalUzivatel.nazev_typ_uzivatele == "Zákazník")
        {
            // open form to create employee
        }
        else if (_originalUzivatel.nazev_typ_uzivatele == "Zaměstnanec")
        {
            // remove employee
            var uzivatel = (UzivatelDTO)editedUzivatel;
            uzivatel.nazev_typ_uzivatele = "Zákazník";
            uzivatel.id_zamestnance = null;
            // sync with the DB here
        }
        UpdateZmenitTypUzivateleSection();
    }

    [RelayCommand]
    private void EditEmployee()
    {
        Zamestnanci zamestnanec = null; // this somehow needs to be the employee linked to the currently edited user
        WindowManager.SetContentView(typeof(ZamestnanciFormViewModel),false,null,new object[]{zamestnanec});
    }

    private void UpdateZmenitTypUzivateleSection()
    {
        var uzivatel = (UzivatelDTO)EditedUzivatel;
        if (uzivatel.nazev_typ_uzivatele == "Zákazník")
        {
            EditBtnVisible = Visibility.Hidden;
            
            ZmenaTypuUzivateleBtnVisible = Visibility.Visible;
            ZmenaTypuUzivateleBtnContent = "Vytvořit zaměstnance";
        }
        else if (uzivatel.nazev_typ_uzivatele == "Zaměstnanec")
        {
            EditBtnVisible = Visibility.Visible;

            ZmenaTypuUzivateleBtnVisible = Visibility.Visible;
            ZmenaTypuUzivateleBtnContent = "Odebrat zaměstnance";
        }
        
    }
    
}