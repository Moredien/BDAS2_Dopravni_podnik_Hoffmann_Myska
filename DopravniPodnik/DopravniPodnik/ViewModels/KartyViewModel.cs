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
    public ObservableCollection<KartyMhd> Items;
    [ObservableProperty]
    public KartyMhd selectedItem;

    private int id;
    private Zakaznici zakaznik;
    private Uzivatele uzivatel;

    // to be finished when the photos can be uploaded
    public KartyViewModel()
    {
        LoadKarty();
    }

    private void LoadKarty()
    {
        // uzivatel = _databaseService.FetchData<Uzivatele>($"SELECT * FROM UZIVATELE WHERE UZIVATELSKE_JMENO = '{UserSession.Instance.UserName}'").FirstOrDefault();
        // zakaznik = _databaseService.FetchData<Zakaznici>(
        //     $"SELECT * FROM ZAKAZNICI WHERE ID_UZIVATELE = {uzivatel.IdUzivatele}").FirstOrDefault();
    }
    [RelayCommand]
    public void Zalozit()
    {
        WindowManager.SetContentView(typeof(ZalozitKartuFormViewModel), new object[] { });
    }
    [RelayCommand]
    public void Informace()
    {
        Console.WriteLine("informace");

    }
    [RelayCommand]
    public void Zaplatit()
    {
        Console.WriteLine("zaplatit");

    }
}