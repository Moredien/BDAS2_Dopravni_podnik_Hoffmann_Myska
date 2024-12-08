using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using DopravniPodnik.ViewModels.Forms;

namespace DopravniPodnik.ViewModels;

public partial class KartyViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService = new();
    [ObservableProperty]
    public ObservableCollection<KartyMhd> items;
    [ObservableProperty]
    public KartyMhd selectedItem;

    private int id;
    private Zakaznici? zakaznik;
    private Uzivatele uzivatel;

    public KartyViewModel()
    {
        Items = new();
        LoadKarty();
    }

    private void LoadKarty()
    {
        var idUzivatele = _databaseService
            .FetchData<Uzivatele>($"SELECT * FROM UZIVATELE WHERE UZIVATELSKE_JMENO = '{UserSession.Instance.UserName}'")
            .FirstOrDefault()?.IdUzivatele;
        zakaznik = _databaseService
            .FetchData<Zakaznici>($"SELECT * FROM ZAKAZNICI WHERE ID_UZIVATELE = {idUzivatele}")
            .FirstOrDefault();
        var data = _databaseService.FetchData<KartyMhd>(
            $"SELECT * FROM KARTY_MHD WHERE ID_ZAKAZNIKA = {zakaznik?.IdZakaznika}");
        foreach (var entry in data)
        {
            Items.Add(entry);
        }
    }
    [RelayCommand]
    public void Zalozit()
    {
        WindowManager.SetContentView(typeof(ZalozitKartuFormViewModel), new object[] { });
    }
    [RelayCommand]
    public void Informace()
    {
        WindowManager.SetContentView(typeof(KartaDetailViewModel), new object[] { selectedItem });

    }
    [RelayCommand]
    public void Dobit()
    {
        WindowManager.SetContentView(typeof(DobitKartuViewModel), new[] { selectedItem });
    }
    public override void Update()
    {
        base.Update();
        WindowManager.ReturnToSelectedContentView();
    }
}