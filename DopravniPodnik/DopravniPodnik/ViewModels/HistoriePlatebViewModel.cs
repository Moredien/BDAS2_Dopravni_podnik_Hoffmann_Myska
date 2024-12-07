using System.Collections.ObjectModel;
using System.Runtime.InteropServices.ComTypes;
using CommunityToolkit.Mvvm.ComponentModel;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;

namespace DopravniPodnik.ViewModels;

public partial class HistoriePlatebViewModel : ViewModelBase
{
    private DatabaseService _databaseService = new();
    [ObservableProperty] private ObservableCollection<Platby> items;
    [ObservableProperty] private ObservableCollection<Platby> filteredItems;

    public HistoriePlatebViewModel()
    {
        Items = new();
        FilteredItems = new();
        var uzivatel =
            _databaseService.FetchData<Uzivatele>(
                $"SELECT * FROM UZIVATELE WHERE UZIVATELSKE_JMENO = '{UserSession.Instance.UserName}'")
                .FirstOrDefault();
        var zakaznik =
            _databaseService.FetchData<Zakaznici>($"SELECT * FROM ZAKAZNICI WHERE ID_UZIVATELE = {uzivatel.IdUzivatele}")
                .FirstOrDefault();
        var data = _databaseService.FetchData<Platby>($"SELECT * FROM PLATBY WHERE ID_ZAKAZNIKA = {zakaznik.IdZakaznika}");
        foreach (var entry in data)
        {
            Items.Add(entry);
        }

        FilterItems();
    }

    private void FilterItems()
    {
        FilteredItems.Clear();
        foreach (var item in Items)
        {
            FilteredItems.Add(item);
        }
    }

    public override void Update()
    {
        base.Update();
        WindowManager.ReturnToSelectedContentView();
    }
}