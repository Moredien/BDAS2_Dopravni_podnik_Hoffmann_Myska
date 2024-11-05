using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using DopravniPodnik.ViewModels.Forms;

namespace DopravniPodnik.ViewModels;

public partial class UzivateleViewModel : ViewModelBase
{
    public ObservableCollection<UzivatelDTO> Uzivatele { get; set; }

    [ObservableProperty]
    private UzivatelDTO selectedUzivatel;

    public UzivateleViewModel()
    {
        SelectedUzivatel = selectedUzivatel;
        Uzivatele = new ObservableCollection<UzivatelDTO>();
        
        Uzivatele.Add(new UzivatelDTO()
        {
            uzivatelske_jmeno = "Tom69",
            heslo = "password",
            jmeno = "Tom",
            prijmeni = "Scott",
            datum_zalozeni = new DateTime(2021,5,24,12,02,11),
            datum_narozeni  = new DateTime(1980,4,21),
            nazev_typ_uzivatele = "user",
            id_foto = 1,
            foto_jmeno_souboru = "foto.png",
            foto_data = null,
            foto_datum_pridani = DateTime.Now,
            id_adresy = 5,
            mesto = "London",
            ulice = "Bell End",
            cislo_popisne = 12345,
            id_zakaznika = 1,
            id_zamestnance = null
        }); 
        Uzivatele.Add(new UzivatelDTO()
        {
            uzivatelske_jmeno = "JaneDoe42",
            heslo = "secure123",
            jmeno = "Jane",
            prijmeni = "Doe",
            datum_zalozeni = new DateTime(2022, 3, 15, 9, 30, 45),
            datum_narozeni = new DateTime(1995, 7, 10),
            nazev_typ_uzivatele = "admin",
            id_foto = 2,
            foto_jmeno_souboru = "jane_profile.jpg",
            foto_data = null,
            foto_datum_pridani = DateTime.Now,
            id_adresy = 8,
            mesto = "New York",
            ulice = "5th Avenue",
            cislo_popisne = 32545,
            id_zakaznika = null,
            id_zamestnance = 2
        });

        Uzivatele.Add(new UzivatelDTO()
        {
            uzivatelske_jmeno = "SamTheMan",
            heslo = "mypassword!",
            jmeno = "Sam",
            prijmeni = "Wilson",
            datum_zalozeni = new DateTime(2023, 1, 10, 15, 45, 23),
            datum_narozeni = new DateTime(1987, 11, 5),
            nazev_typ_uzivatele = "zamestnanec",
            id_foto = 3,
            foto_jmeno_souboru = "sam_pic.png",
            foto_data = null,
            foto_datum_pridani = DateTime.Now,
            id_adresy = 12,
            mesto = "Los Angeles",
            ulice = "Hollywood Blvd",
            cislo_popisne = 12486,
            id_zakaznika = 3,
            id_zamestnance = null
        });

    }

    [RelayCommand]
    void Edit()
    {
        if (SelectedUzivatel == null)
        {
            Console.WriteLine("Neni vybran zadny uzivatel");
            return;
        }
        WindowManager.OpenNewFormView(typeof(UzivatelFormViewModel),new object[]{selectedUzivatel,Uzivatele});
    }
    [RelayCommand]
    void Create()
    {
        WindowManager.OpenNewFormView(typeof(UzivatelFormViewModel),new object[]{Uzivatele});
    }
    [RelayCommand]
    void Delete()
    {
        if(SelectedUzivatel!=null)
            Uzivatele.Remove(SelectedUzivatel);
    }
}